import { Component, Input, ViewChild } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { AuthService } from 'src/app/auth/auth.service';
import { DomainService, ProjectService } from 'src/app/data/service';
import { createGridDataSource } from 'src/app/shared/data-sources/data-sources';
import { DxDataGridHelper } from 'src/app/shared/devexpress/helpers/dx-data-grid-helper';
import { User } from 'src/app/data/schema/user';
import ArrayStore from 'devextreme/data/array_store';
import { DxDataGridComponent } from 'devextreme-angular';
import { GridRowEvent } from 'src/app/shared/devexpress/event-models/grid-row-event';
import { Project, ProjectUser } from 'src/app/data/schema';
import { DxDataGridEvents } from 'src/app/shared/devexpress/constants/dx-data-grid-events';
import { DxFormItemEditorOptions } from 'src/app/shared/devexpress/constants/dx-form-item-editor-options';

@Component({
    selector: 'app-projects-grid',
    templateUrl: './projects-grid.component.html',
})
export class ProjectsGridComponent {
    users: User[];
    proxyUsers: User[];
    ownerId?: number;
    proxyId?: number;
    selectedUsers: number[];
    projectData: Project = null;
    userDataSource: DataSource;

    @ViewChild('usersGrid') usersGrid: DxDataGridComponent;
    @ViewChild('projectsGrid') projectsGrid: DxDataGridComponent;
    @Input() gridId: string;
    @Input() domainRow: any;
    editorOptions = DxFormItemEditorOptions;
    private dataSource: DataSource;
    private get isEditingProject(): boolean {
        return this.projectData != null;
    }

    constructor(
        private authService: AuthService,
        private domainService: DomainService,
        private projectService: ProjectService
    ) {}

    getGridAttributes = (): any => ({ id: this.gridId, style: 'max-height:500px' });

    getDataSource(): DataSource {
        if (!this.dataSource) {
            this.dataSource = createGridDataSource(
                { key: 'id', entityName: 'Project' },
                this.projectService,
                () => this.domainService.getProjectsByDomainId(this.domainRow.key)
            );
        }
        return this.dataSource;
    }

    onProjectsGridInitialized(e: any): void {
        new DxDataGridHelper(e.component)
            .sendAllFieldsOnUpdate()
            .fillMasterDataObject('domain', this.domainRow)
            .fillMasterKey('domainId', this.domainRow)
            .hideToolbar()
            .applyRowNavigationBehavior()
            .resizeOnContentReady();

        this.domainService.getUsersByDomainId(this.domainRow.data.id).subscribe((users) => {
            this.users = users;
            this.buildProxyUserList();
            this.buildUsersDataSource();
        });

        e.component.on(DxDataGridEvents.RowUpdating, this.onProjectsGridRowUpdating.bind(this));
    }

    onProjectsGridRowInserting(e: GridRowEvent<Project>): void {
        e.data.users = this.getSelectedProjectUsers();
        e.data.ownerId = this.ownerId;
        e.data.proxyId = this.proxyId;
    }

    onProjectsGridRowUpdating(e: GridRowEvent<Project>): void {
        e.newData.users = this.getSelectedProjectUsers();
        e.newData.ownerId = this.ownerId;
        e.newData.proxyId = this.proxyId;
    }

    onProjectsGridEditingStart(e: GridRowEvent<Project>): void {
        this.projectData = e.data;
        this.selectedUsers = e.data.users.map((u) => u.userId);
        this.ownerId = e.data.ownerId;
        this.proxyId = e.data.proxyId;
        this.buildProxyUserList();
        this.buildUsersDataSource();
    }

    onProjectsGridInitNewRow(e: GridRowEvent<Project>): void {
        this.projectData = null;
        e.data.isBillable = false;
    }

    onOwnerChanged(): void {
        this.proxyId = null;
        this.buildProxyUserList();
        this.selectOwnerAndProxyUsers();
        this.usersGrid.instance.repaint();
        this.updateProjectData('ownerId', this.ownerId);
    }

    onProxyChanged(): void {
        this.selectOwnerAndProxyUsers();
        this.usersGrid.instance.repaint();
        this.updateProjectData('proxyId', this.proxyId);
    }

    onUserGridEditorPreparing(e): void {
        if (e.command === 'select') {
            if (e.parentType === 'dataRow' && e.row) {
                if (this.isOwnerOrProxy(e.row.data) || e.row.data.pendingTasks) {
                    e.editorOptions.disabled = true;
                    e.editorOptions.value = true;
                }
            } else if (e.parentType === 'headerRow') {
                e.editorOptions.onValueChanged = (event): void => {
                    event.component.visible = false;
                };
                e.editorOptions.visible = false;
            }
        }
    }

    onUserGridSelectionChanged(): void {
        // When a user clicks a disabled checkbox on the grid, it actually changes the selection, so we need to reselect them
        this.selectOwnerAndProxyUsers();
        this.selectUsersWithPendingTasks();
        this.updateProjectData('users', this.getSelectedProjectUsers());
    }

    private getSelectedProjectUsers(): ProjectUser[] {
        return this.selectedUsers.map(
            (userId) => ({ userId, projectId: this.projectData?.id } as ProjectUser)
        );
    }

    private updateProjectData(dataField, value): void {
        if (this.isEditingProject) {
            const editingRowIndex = this.projectsGrid.instance.getRowIndexByKey(
                this.projectData.id
            );
            this.projectsGrid.instance.cellValue(editingRowIndex, dataField, value);
        }
    }

    private isOwnerOrProxy(user): boolean {
        return user.userId === this.ownerId || user.userId === this.proxyId;
    }

    private selectOwnerAndProxyUsers(): void {
        const userIdsToSelect = [this.ownerId ?? 0, this.proxyId ?? 0].filter((id) => id > 0);

        if (userIdsToSelect.length) {
            this.usersGrid.instance.selectRows(userIdsToSelect, true);
        }
    }

    private selectUsersWithPendingTasks(): void {
        if (this.isEditingProject) {
            const usersWithPendingTasks = this.projectData.users
                .filter((u) => u.pendingTasks > 0)
                .map((u) => u.userId);
            this.usersGrid.instance.selectRows(usersWithPendingTasks, true);
        }
    }

    private buildProxyUserList(): void {
        this.proxyUsers = [];

        if (this.users) {
            this.users.forEach((u) => {
                if (u.id !== this.ownerId) {
                    this.proxyUsers.push(u);
                }
            });
        }
    }

    private buildUsersDataSource(): void {
        const users = this.users.map((u) => {
            const projectUser = { user: u, userId: u.id } as ProjectUser;
            if (this.isEditingProject) {
                projectUser.pendingTasks = this.projectData.users.find(
                    (user) => user.userId === u.id
                )?.pendingTasks;
            }
            return projectUser;
        });
        this.userDataSource = new DataSource({
            store: new ArrayStore({
                key: 'userId',
                data: users,
            }),
        });
    }
}
