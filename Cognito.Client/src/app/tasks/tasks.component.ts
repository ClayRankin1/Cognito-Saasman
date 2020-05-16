import { Component, enableProdMode, OnInit, ViewChild, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonService } from '../shared/services/common.service';
import { ITasksGridChanges } from '../shared/services/common.service';
import { AuthService } from '../auth/auth.service';
import DataGrid from 'devextreme/ui/data_grid';
import Popup from 'devextreme/ui/popup';
import DxMenu from 'devextreme/ui/menu';
import { TeamDataService } from './tasks.service';
import { DxDataGridComponent } from 'devextreme-angular';
import { confirm, alert } from 'devextreme/ui/dialog';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { CognitoTimePipe } from '../shared/sharedmodule/datepipe.module';
import { byTextAscending, getLookupNameById, removeMultiRowsInCustomStore } from '../shared/sharedmodule/methods';
import { ProjectLookup } from '../shared/models/project';
import {
    TimeLookup,
    StatusLookup,
    OwnerLookup,
    TeamLookup,
    TaskRowData,
    TypeLookup,
} from './tasks.model';
import { MenuList } from '../shared/models/menulist';
import { LookupService } from '../data/service/lookup.service';
import { LookupType, TaskTypeLookup } from '../data/schema/lookup';
import { CreateRestAspNetStore } from '../shared/data-sources/data-sources';
import { ProjectService } from '../data/service/project.service';

type TaskType = {
    label: string;
};

if (!/localhost/.test(document.location.host)) {
    enableProdMode();
}

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: 'tasks.component.html',
    selector: 'app-tasks',
    styleUrls: ['tasks.component.scss'],
})
export class TasksComponent implements OnInit {
    @ViewChild(DxDataGridComponent)
    dataGrid: DxDataGridComponent;
    store: CustomStore;
    url: string;
    projectsLookup: ProjectLookup[];
    taskTypeLookup: TypeLookup[];
    a_TaskTypeLookup: TypeLookup[];
    timesLookup: TimeLookup[];
    allWhens: TimeLookup[];
    statusLookup: StatusLookup;
    owners: OwnerLookup[];
    workers: TeamLookup[] = [];
    allWorkers: TeamLookup[];
    teamSelected: number[];
    taskName: string;
    teamName: string;
    clickFlag: boolean;
    dataGridInstance: DataGrid;
    menuInstance: DxMenu;
    dueditable: boolean;
    selectedItemKeys: number[] = [];
    startAccruedTime: string;
    endAccruedTime: string;
    clickedRowIndex: number;
    clickedRowID: number;
    movePopupVisible: boolean = false;
    moveyear: number;
    movemonth: number;
    moveday: number;
    movedate: string;
    clickCount: number;
    clonePopupVisible: boolean = false;
    cloneOptions: string[];
    selectedProject: number;
    selectedCloneRadio: string;
    selectedRowData: TaskRowData;
    clickedRowProject: number;
    selectedDate: string;
    savedData: TaskRowData;
    saveCopyClicked: boolean;
    doubleClicked: boolean;
    filterVisible: boolean;
    filterClickedCount: number;
    storeData: TaskRowData[];
    taskPanelOnly: boolean;
    dueMinDate: Date;
    accruedPattern: string = '^([0-1][0-9]|[2][0-3]):([0-5][0-9])$';
    editingRowIndex: number;
    popupInstance: Popup;
    popupType = '';
    cognitoTimePipe = new CognitoTimePipe();
    menuList: MenuList[];
    focusedRowKey: number;
    @Output() gridClear = new EventEmitter<void>();

    ngOnInit(): void {
        this.getTasks('Pending', null);
        this.getTeamsAll();
    }

    constructor(
        private teamDataService: TeamDataService,
        private commonService: CommonService,
        private http: HttpClient,
        private auth: AuthService,
        private lookupService: LookupService,
        private projectService: ProjectService
    ) {
        this.dueditable = true;
        this.clickFlag = true;
        this.clickCount = 0;
        this.clickedRowIndex = -1;
        this.selectedProject = 0;
        this.saveCopyClicked = false;
        this.doubleClicked = false;
        this.filterVisible = false;
        this.filterClickedCount = 0;
        this.taskPanelOnly = false;
        this.storeData = [];
        this.menuList = [
            {
                id: '1',
                name: 'New',
                icon: '',
                items: [],
            },
            {
                id: '2',
                name: 'Edit',
                icon: '',
                items: [],
            },
            {
                id: '3',
                name: 'View',
                icon: '',
                items: [
                    {
                        id: '3_1',
                        name: 'Toggle Filter Row',
                        icon: 'isblank',
                        items: [],
                    },
                    {
                        id: '3_2',
                        name: 'Popup Edit',
                        icon: 'check',
                        items: [],
                    },
                    {
                        id: '3_3',
                        name: 'Cell Edit',
                        icon: 'isblank',
                        items: [],
                    },
                ],
            },
            {
                id: '4',
                name: 'Complete',
                icon: '',
                items: [],
            },
            {
                id: '5',
                name: 'Move',
                icon: '',
                items: [],
            },
            {
                id: '6',
                name: 'Advance',
                icon: '',
                items: [],
            },
            {
                id: '7',
                name: 'Delete',
                icon: '',
                items: [],
            },
            {
                id: '8',
                name: 'Report',
                icon: '',
                items: [
                    {
                        id: '8_1',
                        name: 'TimeSheet',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_2',
                        name: 'Show Export Grid',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_3',
                        name: 'Hide Export Grid',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_4',
                        name: 'Activities Report',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_5',
                        name: 'Activities Detail Report',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '9',
                name: 'Copy',
                icon: '',
                items: [
                    {
                        id: '9_1',
                        name: 'Clone',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '9_2',
                        name: 'Repeat',
                        icon: '',
                        items: [],
                    },
                ],
            },
        ];

        let today = new Date();
        this.dueMinDate = new Date(today.setDate(today.getDate() - 1));

        this.lookupService.getLookup(LookupType.Time).subscribe((result) => {
            const timesLookup = new DataSource({
                store: new ArrayStore({
                    data: result,
                }),
                paginate: false,
            });
            timesLookup.load();
            this.timesLookup = timesLookup.items();
            this.allWhens = this.timesLookup;
        });

        this.cloneOptions = ['Clone Task Only', 'Clone Task and All Related Info'];

        this.statusLookup = {
            store: new CustomStore({
                key: 'id',
                loadMode: 'raw',
                load: () => {
                    return this.lookupService.getLookup(LookupType.TaskStatus).toPromise();
                },
                onLoaded() { },
            }),
        };

        this.projectService.getAll().subscribe((projects) => {
            const projectsLookup = new DataSource({
                store: new ArrayStore({
                    data: projects.map((p) => {
                        return {
                            id: p.id,
                            name: p.nickname,
                        };
                    }),
                }),
                paginate: false,
            });
            projectsLookup.load();
            this.projectsLookup = projectsLookup.items();
        });

        this.lookupService.getLookup<TaskTypeLookup>(LookupType.TaskType).subscribe((lookup) => {
            const taskTypeLookup = new DataSource({
                store: new ArrayStore({
                    data: lookup,
                }),
                paginate: false,
            });
            taskTypeLookup.load();
            this.taskTypeLookup = taskTypeLookup.items();
            this.a_TaskTypeLookup = this.taskTypeLookup;
        });

        this.teamCellTemplate = this.teamCellTemplate.bind(this);
    }

    getTasks = (status: string, projectId: number): void => {
        const auth = this.auth;
        const that: any = this;
        this.store = CreateRestAspNetStore({
            entityName: 'Task',
            key: 'id',
            url: environment.activeURL + 'api/Tasks',
            loadUrlSuffix: '/project',
            loadParams: { status, projectId: projectId },
            authService: auth,
            onLoaded(result: any[]) {
                that.storeData = result;
            },
        });
    };

    onRowClick = (e): void => {
        if (e.rowType === 'data') {
            this.dataGridInstance.deselectRows(e.data.id);
            if (this.clickCount < 2) {
                this.clickCount++;
            } else {
                this.clickCount = 1;
            }

            setTimeout(() => {
                if (this.clickCount > 1) {
                    // Stop clicking of checkbox
                    this.doubleClicked = true;

                    e.component.collapseAll(-1);
                    if (this.clickedRowID == e.data.id && !this.clickFlag) {
                        this.clickFlag = true;
                    } else {
                        e.component.expandRow(e.data.id);
                        this.clickFlag = false;
                    }
                    this.clickedRowID = e.data.id;
                    localStorage.setItem('projectId', e.data.projectId);
                } else {
                    if (this.clickCount > 0) {
                        if (
                            e.data.id !== parseInt(localStorage.getItem('taskId')) &&
                            this.dataGridInstance.option('editing.mode') === 'popup'
                        ) {
                            let pRowIndex: number = e.component.getRowIndexByKey(
                                +localStorage.getItem('taskId')
                            );
                            e.component
                                .getRowElement(pRowIndex)[0]
                                .classList.remove('dx-double-click');
                            e.component
                                .getRowElement(e.rowIndex)[0]
                                .classList.add('dx-double-click');
                            this.getRelatedInfo(e.data.id, e.data.projectId, e.data.detailTypeId);
                            e.event.preventDefault();
                            localStorage.setItem('taskId', e.data.id);
                            localStorage.setItem('projectId', e.data.projectId);
                            document.getElementById('toggle-clock-id').style.display = 'block';
                            document.getElementById('text-clock-id').setAttribute('times', '0');
                            document.getElementById('text-clock-id').innerHTML = '00:00:00';
                            document.getElementById('clock').classList.remove('icon-clock');
                        }
                    }
                }
                this.clickCount = 0;
            }, 300);
        }
    };

    onRowDblClick = (e): void => { };

    contentReady = (e): void => {
        this.dataGridInstance.deselectAll();
        this.focusedRowKey = +localStorage.getItem('taskId');
        // Add "dx-double-click" class to the expanded row.
        if (+localStorage.getItem('taskId')) {
            let rowIndex: number = e.component.getRowIndexByKey(+localStorage.getItem('taskId'));
            if (rowIndex >= 0) {
                e.component.getRowElement(rowIndex)[0].classList.add('dx-double-click');
            }
        }
    };

    selectionChanged = (e): void => {
        this.selectedItemKeys = e.selectedRowKeys;
        // Get rowData of selected row
        this.selectedRowData = this.dataGridInstance.getSelectedRowsData()[0];
        // In the case of multiple selections, the Delete All button is active and the Single Delete button is disabled.
        // In the case of a single choice, the opposite is true.
        if (this.selectedItemKeys.length > 1) {
            document
                .getElementById('multi_del_btn')
                .parentElement.classList.remove('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'none';
            });
        } else {
            document
                .getElementById('multi_del_btn')
                .parentElement.classList.add('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'unset';
            });
        }
    };

    teamCellTemplate(container, options): void {
        let ids: number[] = options.value;
        var noBreakSpace: string = '\u00A0',
            text: string = this.getTeamNamebyIds(ids);
        container.textContent = text || noBreakSpace;
        container.title = text;
    }

    calculateFilterExpression(filterValue: number, target: string) {
        return function (data: TaskRowData): boolean {
            return (data.subtasks || []).includes(filterValue);
        };
    }

    onRowUpdated(e): void {
        this.savedData = e.data;
        this.taskTypeLookup = this.a_TaskTypeLookup;
    }

    onRowInserted = (e): void => {
        localStorage.setItem('taskId', e.key);
        this.getRelatedInfo(e.key, e.data.projectId, e.data.detailTypeId);
        this.savedData = e.data;
        this.taskTypeLookup = this.a_TaskTypeLookup;
    };

    onRowInserting = (e): void => { };

    usagesGrid_onRowUpdating = (e): void => {
        // sends all the data to BE and not just updated properties
        e.newData = Object.assign(e.oldData, e.newData);
    };

    onInitNewRow = (e): void => {
        //TO DO - SET POPUP TITLE TO "Add Tasks"
        this.dataGridInstance.option('editing.popup.title', 'Add Task');
        this.getTeams(+localStorage.getItem('projectId'));
        e.data.ownerId = this.auth.currentUser.userId;

        this.popupType = 'insert';

        // Initialize new row when user clicks 'Save+Copy' bubtton
        if (this.saveCopyClicked) {
            if (this.savedData) {
                e.data = { ...this.savedData };
            }
        } else {
            e.data.isEvent = false;
            this.timesLookup = this.getRestTimes();
            let t_taskTypeLookup: TypeLookup[] = [];
            this.a_TaskTypeLookup.forEach((element) => {
                if (element.label !== element.label.toUpperCase()) {
                    t_taskTypeLookup.push(element);
                }
            });
            this.taskTypeLookup = t_taskTypeLookup;
            //TO DO - SET STATUS TO "Pending"
            e.data.status = 'Pending';

            // TODO - SET DATE TO TODAY
            e.data.nextDate = new Date();

            //TO DO - SET WHEN TO "3-Today"
            let todayWhenId: number = 0;
            this.allWhens.forEach((element) => {
                if (element.label.trim() === '3-Today') {
                    todayWhenId = element.id;
                }
            });
            e.data.timeId = todayWhenId;

            // TODO - SET TASK TO 'Work on project.'
            e.data.description = 'Work on the project.';

            //TO DO - SET PROJECT TO THE MOST RECENT ONE
            let projectId: number = +localStorage.getItem('projectId');
            let projectIds: number[] = [];
            this.projectsLookup.forEach((element) => {
                projectIds.push(element.id);
            });
            if (projectIds.includes(projectId)) {
                e.data.projectId = projectId;
            } else {
                e.data.projectId = '';
            }

            //TO DO - SET TEAM TO CURRENT USER
            for (let index: number = 0; index < this.workers.length; index++) {
                if (this.workers[index].fullName === this.auth.currentUser.userName) {
                    let subtasks: number[] = [];
                    subtasks[0] = this.workers[index].id;
                    e.data.subtasks = subtasks;
                }
            }
        }
    };

    onShowing = (e): void => {
        this.popupInstance = e.component;
        const items = e.component.option('toolbarItems');
        const btnSaveNext = items.find((x) => x.name === 'SaveNext');
        if (this.popupType === 'insert') {
            btnSaveNext.options.visible = false;
        } else if (this.popupType === 'edit') {
            btnSaveNext.options.visible = true;
        }
    };

    editstart = (e): void => {
        //TO DO - SET POPUP TITLE TO "Tasks Edit"
        if (this.dataGridInstance.option('editing.mode') === 'popup') {
            this.dataGridInstance.option('editing.popup.title', 'Edit Task');
        }
        this.getTeams(e.data.projectId);

        this.popupType = 'edit';

        let t_taskTypeLookup: TypeLookup[] = [];
        if (e.data.isEvent) {
            this.timesLookup = this.getEventTimes();
            this.a_TaskTypeLookup.forEach((element) => {
                if (element.label === element.label.toUpperCase()) {
                    t_taskTypeLookup.push(element);
                }
            });
            this.taskTypeLookup = t_taskTypeLookup;
        } else {
            this.timesLookup = this.getRestTimes();
            this.a_TaskTypeLookup.forEach((element) => {
                if (element.label !== element.label.toUpperCase()) {
                    t_taskTypeLookup.push(element);
                }
            });
            this.taskTypeLookup = t_taskTypeLookup;
        }
        localStorage.setItem('projectId', e.data.projectId);
        this.editingRowIndex = this.dataGridInstance.getRowIndexByKey(e.key);
    };

    teamloadata = (cellInfo): void => {
        if (typeof cellInfo.value === 'string') this.teamSelected = JSON.parse(cellInfo.value);
        else if (typeof cellInfo.value === 'object') this.teamSelected = cellInfo.value;
    };

    getProjectNamebyID = (projectId: number): string => {
        return getLookupNameById(this.projectsLookup, projectId, 'id', 'name');
    };

    getTimeNamebyID = (timeId: number): string => {
        return getLookupNameById(this.allWhens, timeId);
    };

    getTypeNamebyID = (typeId: number): string => {
        return getLookupNameById(this.a_TaskTypeLookup, typeId);
    };

    getOwnerNamebyID = (ownerId: number): string => {
        return getLookupNameById(this.allWorkers, ownerId, 'id', 'fullName');
    };

    getTeamNamebyIds = (ids: number[]): string => {
        if (ids.length > 0) {
            let teamNameArray: string[] = [];
            this.allWorkers.forEach((element) => {
                for (let index = 0; index < ids.length; index++) {
                    if (element.id === ids[index]) {
                        teamNameArray.push(element.fullName);
                    }
                }
            });
            this.teamName = teamNameArray.join(', ');
            return this.teamName;
        } else {
            return "";
        }
    };

    saveGridInstance = (e): void => {
        this.dataGridInstance = e.component;
    };
    onSave = (e): void => {
        this.dataGridInstance.saveEditData();
    };
    onSaveNew = (e): void => {
        this.saveCopyClicked = false;
        this.dataGridInstance.saveEditData().then(() => {
            if (!this.dataGridInstance.hasEditData()) {
                this.dataGridInstance.addRow();
            }
        });
    };
    onSaveCopy = (e): void => {
        this.saveCopyClicked = true;
        this.dataGridInstance.saveEditData().then(() => {
            if (!this.dataGridInstance.hasEditData()) {
                this.dataGridInstance.addRow();
            }
        });
    };

    onSaveNext = (e): void => {
        let nextRowIndex: number = this.editingRowIndex + 1;
        let nextRowElement: Element[] = this.dataGridInstance.getRowElement(nextRowIndex);
        if (!nextRowElement) {
            this.dataGridInstance.saveEditData().then();
            return;
        }
        let nextRowElementClassList: DOMTokenList = nextRowElement[0].classList;
        for (var i = nextRowElementClassList.length >>> 0; i--;) {
            if (nextRowElementClassList[i] === 'dx-group-row') {
                nextRowIndex++;
            }
        }
        this.dataGridInstance.saveEditData().then(() => {
            if (!this.dataGridInstance.hasEditData()) {
                this.dataGridInstance.editRow(nextRowIndex);
            }
        });
    };

    onCancel = (e): void => {
        this.taskTypeLookup = this.a_TaskTypeLookup;
        this.timesLookup = this.allWhens;
        this.owners = this.allWorkers;
        this.workers = this.allWorkers;
        this.dataGridInstance.cancelEditData();
    };

    getTeamsAll = (): void => {
        this.http
            .get(
                environment.activeURL + 'api/Domains/' + this.auth?.currentUser?.domainId + '/team'
            )
            .subscribe((result: TeamLookup[]) => {
                const allTeamLookup = new DataSource({
                    store: new ArrayStore({
                        data: result,
                    }),
                    paginate: false,
                });
                allTeamLookup.load();
                this.allWorkers = allTeamLookup.items();
                this.workers = allTeamLookup.items();
                this.owners = allTeamLookup.items();
            });
    };

    getTeams = (projectId: number): void => {
        if (!isNaN(+projectId) && projectId) {
            this.http
                .get(environment.activeURL + 'api/Projects/' + projectId + '/team')
                .subscribe((result: TeamLookup[]) => {
                    const teamLookup = new DataSource({
                        store: new ArrayStore({
                            data: result,
                        }),
                        paginate: false,
                    });
                    teamLookup.load();
                    this.workers = teamLookup.items();
                    this.owners = teamLookup.items();
                });
        }
    };
    insertAccrued = (): void => {
        this.teamDataService
            .InsertAccruedTime(
                this.startAccruedTime,
                this.endAccruedTime,
                +localStorage.getItem('taskId')
            )
            .subscribe((data) => {
                this.dataGridInstance.refresh();
                this.startAccruedTime = null;
                this.endAccruedTime = null;
            });
    };

    onEditorPreparing = (e): void => {
        if (e.parentType === 'dataRow' && e.dataField === 'ownerId') {
        }

        if (e.parentType === 'dataRow' && e.dataField === 'projectId') {
            const standardHandler = e.editorOptions.onValueChanged;
            e.editorOptions.onValueChanged = (args) => {
                localStorage.setItem('projectId', args.value);
                standardHandler(args);
                this.getTeams(args.value);
            };
        }
        //TO-DO TRY TO MAKE CALENDAR DROP ON FOCUS ....dx-dropdowneditor-active
        if (e.parentType === 'dataRow' && e.dataField === 'isEvent') {
            if (e.row.isNewRow) e.editorOptions.disabled = false;
            else e.editorOptions.disabled = true;
            var standardHandler = e.editorOptions.onValueChanged;
            e.editorOptions.onValueChanged = (args) => {
                standardHandler(args);
                let t_taskTypeLookup: TypeLookup[] = [];
                if (args.value) {
                    this.timesLookup = this.getEventTimes();
                    this.a_TaskTypeLookup.forEach((element) => {
                        if (element.label === element.label.toUpperCase()) {
                            t_taskTypeLookup.push(element);
                        }
                    });
                    this.taskTypeLookup = t_taskTypeLookup;
                } else {
                    this.timesLookup = this.getRestTimes();
                    this.a_TaskTypeLookup.forEach((element) => {
                        if (element.label !== element.label.toUpperCase()) {
                            t_taskTypeLookup.push(element);
                        }
                    });
                    this.taskTypeLookup = t_taskTypeLookup;
                }
                e.component.cellValue(e.row.rowIndex, 'timeId', null);
                e.component.cellValue(e.row.rowIndex, 'taskTypeId', null);
            };
        }
        if (
            e.parentType === 'dataRow' &&
            (e.dataField === 'nextDate' || e.dataField === 'endDate')
        ) {
            e.editorOptions.openOnFieldClick = true;
        }

        const taskComponent = this;
        document.querySelectorAll('.btn').forEach((el) => {
            el.addEventListener('click', function (event) {
                const expandColumns: string[] = [
                    'nextDate',
                    'timeId',
                    'endDate',
                    'projectId',
                    'taskTypeId',
                    'description',
                    'detailsCount',
                    'subtasks',
                ];
                if (
                    document.getElementsByClassName('btnDown').length == 1 &&
                    document.getElementsByClassName('btnDown')[0].id == 'btn_tasks'
                ) {
                    expandColumns.forEach((element) => {
                        e.component.columnOption(element, 'visible', true);
                    });
                    e.component.columnOption('virtualcol', 'visible', false);
                    e.component.columnOption('accrued', 'caption', 'Accrued');

                    taskComponent.taskPanelOnly = true;
                    e.component.columnOption('accrued', 'width', 60);
                } else {
                    expandColumns.forEach((element) => {
                        e.component.columnOption(element, 'visible', false);
                    });
                    e.component.columnOption('virtualcol', 'visible', true);
                    e.component.columnOption('accrued', 'caption', 'Hrs');

                    taskComponent.taskPanelOnly = false;
                    e.component.columnOption('accrued', 'width', 100);
                }
            });
        });

        if (e.parentType === 'filterRow' && e.dataField === 'taskTypeId') {
            e.editorOptions.dataSource = this.getAvailableTypeLookup();
        }
    };

    getstartdata = (startdate: string): string => {
        if (startdate) {
            let t_startdate: string[] = startdate.split('T');
            return t_startdate[0];
        } else {
            return '';
        }
    };

    getendate = (endate: string, when: number): string => {
        if (endate) {
            const t_endate = endate.split('T');
            return t_endate[0];
        } else {
            return '';
        }
    };

    deleteRecords = (e): void => {
        if (this.selectedItemKeys.length) {
            const result = confirm('Are you sure you want to delete the selected tasks? This will also delete all details associated with the task.', 'Confirm');
            result.then((dialogResult: boolean) => {
                if (dialogResult) {
                    let removedRowCount: number = 0;
                    this.selectedItemKeys.forEach((key) => {
                        this.store.remove(key).then(
                            (key) => {
                                removedRowCount++;
                                if (removedRowCount === this.selectedItemKeys.length) {
                                    if (this.selectedItemKeys.includes(+localStorage.getItem('taskId'))) {
                                        localStorage.removeItem('taskId');
                                        this.gridClear.emit();
                                    }
                                    this.dataGridInstance.refresh();
                                }
                            }
                        );
                    });
                }
            });
        } else {
            alert('No record selected.', 'Message');
        }
    };

    onRowRemoved = (e): void => {
        if (e.key === +localStorage.getItem('taskId')) {
            localStorage.removeItem('taskId');
            this.gridClear.emit();
        }
    };

    onCellPrepared = (e): void => {
        let cellElement: Element = e.cellElement;
        if (e.rowType === 'data' && e.column.dataField === 'description') {
            cellElement.addEventListener('mousemove', (ee) => {
                cellElement.setAttribute('title', e.value);
            });
        }
    };

    MenuClick = (data): void => {
        let item: MenuList = data.itemData;
        switch (item.name) {
            case 'New': {
                this.saveCopyClicked = false;
                this.dataGridInstance.addRow();
                break;
            }
            case 'Edit': {
                if (this.selectedItemKeys.length) {
                    let clickedId: number = this.selectedItemKeys[this.selectedItemKeys.length - 1];
                    let clickedIndex: number = this.dataGridInstance.getRowIndexByKey(clickedId);
                    this.dataGridInstance.editRow(clickedIndex);
                } else {
                    alert('No record selected.', 'Message');
                }
                break;
            }
            case 'View': {
                break;
            }
            case 'Complete': {
                if (this.selectedItemKeys.length) {
                    const result = confirm(
                        'Are you sure you want to complete the selected records?',
                        'Confirm'
                    );
                    result.then((dialogResult: Boolean) => {
                        if (dialogResult) {
                            this.selectedItemKeys.forEach((key) => {
                                this.teamDataService.menuActionComplete(key).subscribe((data) => {
                                    this.dataGridInstance.refresh();
                                });
                            });
                        }
                    });
                } else {
                    alert('No record selected.', 'Message');
                }
                break;
            }
            case 'Move': {
                if (this.selectedItemKeys.length) {
                    this.movePopupVisible = true;
                    this.selectedDate = this.selectedRowData.nextDate;
                } else {
                    alert('No record selected.', 'Message');
                }
                break;
            }
            case 'Advance': {
                this.teamDataService.menuActionAdvance().subscribe((data) => {
                    this.dataGridInstance.refresh();
                });
                break;
            }
            case 'Delete': {
                this.deleteRecords(this);
                break;
            }
            case 'Clone': {
                if (this.selectedItemKeys.length) {
                    this.clonePopupVisible = true;
                    this.clickedRowProject = this.selectedRowData.projectId;
                    this.selectedCloneRadio = this.cloneOptions[0];
                } else {
                    alert('No record selected.', 'Message');
                }
                break;
            }
            case 'Toggle Filter Row': {
                this.filterClickedCount++;
                this.filterVisible = this.filterClickedCount % 2 == 0 ? false : true;
                if (this.filterVisible) {
                    item.icon = 'check';
                } else {
                    item.icon = 'isblank';
                    this.dataGridInstance.refresh();
                }
                break;
            }
            case 'Popup Edit': {
                const allitems = this.menuInstance.option('items');
                allitems[2]['items'][2].icon = 'isblank';
                allitems[2]['items'][1].icon = 'check';
                this.dataGridInstance.option('editing.mode', 'popup');
                break;
            }
            case 'Cell Edit': {
                const allitems = this.menuInstance.option('items');
                allitems[2]['items'][1].icon = 'isblank';
                allitems[2]['items'][2].icon = 'check';
                this.dataGridInstance.option('editing.mode', 'cell');
                break;
            }
            default: {
                break;
            }
        }
    };

    menuInitial = (e): void => {
        this.menuInstance = e.component;
    };

    onToolbarPreparing = (e): void => {
        let that = this;
        let counterobj: number;
        let continuetimeobj: number;

        function counter(obj: Element, target: Element): void {
            if (obj.classList.contains('icon-clock')) {
                counterobj = window.setTimeout(() => {
                    let times: number = parseInt(target.getAttribute('times'));
                    target.setAttribute('times', (times + 1).toString());
                    if (that.startAccruedTime == null) {
                        that.startAccruedTime = new Date().toISOString();
                    }
                    var date: Date = new Date(null);
                    date.setSeconds(times + 1);
                    var result: string = date.toISOString().substr(11, 8);
                    target.innerHTML = result;
                    counter(obj, target);
                }, 1000);
            } else {
                that.endAccruedTime = new Date().toISOString();
                that.insertAccrued();
                clearTimeout(continuetimeobj);
            }
        }
        function continuetime(obj: Element, target: Element): void {
            continuetimeobj = window.setTimeout(() => {
                let result = confirm(
                    'Do you want to continue keeping time on this task?',
                    'Confirm'
                );
                clearTimeout(counterobj);
                that.endAccruedTime = new Date().toISOString();
                result.then((dialogResult: Boolean) => {
                    if (dialogResult) {
                        counter(obj, target);
                        continuetime(obj, target);
                    } else {
                        that.insertAccrued();
                        obj.classList.remove('icon-clock');
                    }
                });
            }, 300000);
        }
        e.toolbarOptions.items.unshift(
            {
                location: 'left',
                widget: 'dxButton',
                options: {
                    onClick: this.deleteRecords.bind(this),
                    hint: 'Delete',
                    disabled: true,
                    width: '35x',
                    height: '35px',
                    template(data, element) {
                        element.classList.add('icon-delete');
                        element.id = 'multi_del_btn';
                    },
                },
            },
            {
                location: 'left',
                template: function (): Element {
                    let toggleClock: Element = document.createElement('div');
                    toggleClock.className = 'toggle-clock';
                    toggleClock.id = 'toggle-clock-id';

                    let clock: Element = document.createElement('div');
                    clock.id = 'clock';

                    let clockText: Element = document.createElement('div');
                    clockText.setAttribute('times', '0');
                    clockText.className = 'text-clock';
                    clockText.id = 'text-clock-id';
                    clockText.innerHTML = '00:00:00';

                    clock.addEventListener('click', function (event) {
                        this.classList.contains('icon-clock')
                            ? this.classList.remove('icon-clock')
                            : this.classList.add('icon-clock');
                        if (this.classList.contains('icon-clock')) {
                            counter(this, clockText);
                            continuetime(this, clockText);
                        } else {
                            clearTimeout(continuetimeobj);
                        }
                    });
                    toggleClock.appendChild(clock);
                    toggleClock.appendChild(clockText);
                    return toggleClock;
                },
            }
        );

        // Customize add row event
        const toolbarItems = e.toolbarOptions.items;
        const searchPanel = toolbarItems.find((x) => x.name === 'searchPanel');
        const addRowButton = toolbarItems.find((x) => x.name === 'addRowButton');
        const columnChooserButton = toolbarItems.find((x) => x.name === 'columnChooserButton');
        if (searchPanel) {
            searchPanel.location = 'before';
        }
        if (addRowButton) {
            addRowButton.location = 'before';
            addRowButton.options.onClick = (args) => {
                this.saveCopyClicked = false;
                this.dataGridInstance.addRow();
            };
        }
        if (columnChooserButton) {
            columnChooserButton.location = 'before';
        }
        const nToolbarItems = [];
        if (toolbarItems.length) {
            nToolbarItems[0] = toolbarItems[4];
            nToolbarItems[1] = toolbarItems[2];
            nToolbarItems[2] = toolbarItems[3];
            nToolbarItems[3] = toolbarItems[0];
            nToolbarItems[4] = toolbarItems[1];
        }
        e.toolbarOptions.items = nToolbarItems;
    };
    setvirtualtest = (cellInfo: TaskRowData): string => {
        if (cellInfo) {
            let when: string = '';
            if (cellInfo.timeId) {
                when = this.getTimeNamebyID(cellInfo.timeId);
            }
            return (
                (when || '') +
                ' ' +
                (cellInfo.projectName || '') +
                ' ' +
                (this.getTypeNamebyID(cellInfo.projectTypeId) || '') +
                ' ' +
                (cellInfo.description || '') +
                ' ' +
                (this.getOwnerNamebyID(cellInfo.ownerId) || '') +
                ' ' +
                'team'
            );
        } else {
            return '';
        }
    };
    setgroupdate = (cellInfo): string => {
        if (cellInfo.value) {
            let days: string[] = [
                'Sunday',
                'Monday',
                'Tuesday',
                'Wednesday',
                'Thursday',
                'Friday',
                'Saturday',
            ];
            let dayName: string = days[cellInfo.value.getDay()];
            let monthNames: string[] = [
                'Jan',
                'Feb',
                'Mar',
                'Apr',
                'May',
                'Jun',
                'Jul',
                'Aug',
                'Sep',
                'Oct',
                'Nov',
                'Dec',
            ];
            let monthName: string = monthNames[cellInfo.value.getMonth()];

            return (
                dayName +
                ' ' +
                monthName +
                ' ' +
                cellInfo.value.getDate() +
                ', ' +
                cellInfo.value.getFullYear()
            );
        } else {
            return '';
        }
    };

    onMoveSubmit = (): void => {
        let count: number = 0;
        this.selectedItemKeys.forEach((key) => {
            this.teamDataService.menuActionMove(key, this.movedate).subscribe((data) => {
                count++;
                if (count == this.selectedItemKeys.length) {
                    this.movePopupVisible = false;
                    this.dataGridInstance.refresh();
                }
            });
        });
    };

    onMoveDateValueChanged = (e): void => {
        try {
            this.movedate = e.value.toISOString();
        } catch (error) {
            this.movedate = e.value;
        }
    };

    GotoDetail = (e): void => {
        if (e.row.data.id != parseInt(localStorage.getItem('taskId'))) {
            let pRowIndex: number = e.component.getRowIndexByKey(+localStorage.getItem('taskId'));
            e.component.getRowElement(pRowIndex)[0].classList.remove('dx-double-click');
            e.component.getRowElement(e.row.rowIndex)[0].classList.add('dx-double-click');
            this.getRelatedInfo(e.row.data.id, e.row.data.projectId, e.row.data.detailTypeId);
            e.event.preventDefault();
            localStorage.setItem('taskId', e.row.data.id);
            localStorage.setItem('projectId', e.row.data.projectId);

            document.getElementById('toggle-clock-id').style.display = 'block';
            document.getElementById('text-clock-id').setAttribute('times', '0');
            document.getElementById('text-clock-id').innerHTML = '00:00:00';
            document.getElementById('clock').classList.remove('icon-clock');
        }
        // If you only have the Tasks panel, open the Details panel.
        if (this.taskPanelOnly) {
            document.getElementById('btn_items').click();
            const expandColumns: string[] = [
                'nextDate',
                'timeId',
                'endDate',
                'projectId',
                'taskTypeId',
                'description',
                'detailsCount',
                'subtasks',
            ];
            expandColumns.forEach((element) => {
                this.dataGridInstance.columnOption(element, 'visible', false);
            });
            this.dataGridInstance.columnOption('virtualcol', 'visible', true);
            this.dataGridInstance.columnOption('accrued', 'caption', 'Hrs');

            this.taskPanelOnly = false;
        }
    };

    onCloneProjectChanged = (e): void => {
        this.selectedProject = e.value;
    };
    onCloneRadioChanged = (e): void => {
        this.selectedCloneRadio = e.value;
    };
    onCloneSubmit = (): void => {
        // In the case of "Clone Task Only"
        if (this.selectedCloneRadio === this.cloneOptions[0]) {
            let dataObj: TaskRowData = this.selectedRowData;
            dataObj.projectId = this.selectedProject;
            if (dataObj) {
                const taskComponent = this;
                this.store.insert(dataObj).then(
                    (dataObj) => {
                        taskComponent.clickedRowProject = 0;
                        taskComponent.clonePopupVisible = false;
                        taskComponent.dataGridInstance.refresh();
                    }
                );
            }
        } else {
            // In the case of "Clone Task and All Related Info"
            let dataObj: TaskRowData = this.selectedRowData;
            dataObj.projectId = this.selectedProject;
            if (dataObj) {
                const taskComponent = this;
                this.store.insert(dataObj).then(
                    (dataObj) => {
                        taskComponent.clickedRowProject = 0;
                        taskComponent.clonePopupVisible = false;
                        taskComponent.dataGridInstance.refresh();
                    }
                );
            }

            // Save the related Details
        }
    };

    onCloneCancelClick = (): void => {
        this.clickedRowProject = 0;
        this.clonePopupVisible = false;
    };

    getAvailableTypeLookup = (): TypeLookup[] => {
        if (this.storeData.length > 0) {
            let typeArray: number[] = [];
            let newArray: TypeLookup[] = this.storeData.map((item: TaskRowData) => {
                if (!typeArray.includes(item.taskTypeId)) {
                    const value = {
                        id: item.taskTypeId,
                        label: this.getTypeNamebyID(item.taskTypeId),
                    };
                    typeArray.push(item.taskTypeId);
                    return value;
                }
            });
            newArray = newArray.filter((item) => !!item);
            newArray.sort(byTextAscending((type: TaskType) => type.label));
            return newArray;
        }
    };
    getEventTimes = (): TimeLookup[] => {
        let nEventWhenStartId: number;
        this.allWhens.forEach((element: TimeLookup) => {
            if (element.label.trim() === '1-ASAP') {
                nEventWhenStartId = element.id;
            }
        });

        let eventTimes: TimeLookup[] = [];
        this.allWhens.forEach((element: TimeLookup) => {
            if (element.id < nEventWhenStartId) {
                eventTimes.push(element);
            }
        });
        return eventTimes;
    };
    getRestTimes = (): TimeLookup[] => {
        let nEventWhenStartId: number;
        this.allWhens.forEach((element: TimeLookup) => {
            if (element.label.trim() === '1-ASAP') {
                nEventWhenStartId = element.id;
            }
        });

        let restTimes: TimeLookup[] = [];
        this.allWhens.forEach((element: TimeLookup) => {
            if (element.id >= nEventWhenStartId) {
                restTimes.push(element);
            }
        });

        return restTimes;
    };

    getRelatedInfo(taskId: number, projectId: number, detailTypeId: number): void {
        const changedValues: ITasksGridChanges = {
            projectId: projectId,
            taskId: taskId,
            detailTypeId: detailTypeId,
        };
        this.commonService.updateListFn(changedValues);
    }
}
