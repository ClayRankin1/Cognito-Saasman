import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { DomainService } from 'src/app/data/service';
import { AuthService } from 'src/app/auth/auth.service';
import { createGridDataSource } from 'src/app/shared/data-sources/data-sources';
import { DxDataGridHelper } from 'src/app/shared/devexpress/helpers/dx-data-grid-helper';
import DataGrid from 'devextreme/ui/data_grid';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-projects',
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ProjectsComponent implements OnInit {
    dataSource: any;
    hasOnlyOneDomain = false;
    private projectGridIdPrefix = 'ProjectGrid';

    constructor(private domainService: DomainService, private authService: AuthService) {}

    ngOnInit(): void {
        this.dataSource = createGridDataSource(
            { key: 'id', entityName: 'Domain' },
            this.domainService,
            () =>
                this.domainService.getAdminDomains().pipe(
                    tap((domains) => {
                        this.hasOnlyOneDomain = domains.length === 1;
                    })
                )
        );
    }

    onGridInitialized(e): void {
        new DxDataGridHelper(e.component).expandRowOnClick().oneRowExpandedAtaTime();
    }

    onGridCellPrepared(e): void {
        if (e.rowType === 'data' && e.column.type === 'buttons') {
            if (!this.authService.isAdminForDomain(e.data.id)) {
                for (const element of e.cellElement.children) {
                    element.remove();
                }
            }
        }
    }

    calculateDomainName(rowData): string {
        return rowData.tenant.name + ' - ' + rowData.name;
    }

    addProjectRow = (e: any): void => {
        if (!e.row.isExpanded) {
            e.component.expandRow(e.row.key);
        }

        const element = document.getElementById(this.getProjectsGridId(e.row));
        const instance = DataGrid.getInstance(element) as DataGrid;
        instance.addRow();
    };

    onGridContentReady(e): void {
        if (e.component.totalCount() === 1 && !e.component.isNotFirstLoad) {
            e.component.isNotFirstLoad = true;
            e.component.expandAll();
        }
    }

    getProjectsGridId(domainRow: any): string {
        return this.projectGridIdPrefix + domainRow.key;
    }
}
