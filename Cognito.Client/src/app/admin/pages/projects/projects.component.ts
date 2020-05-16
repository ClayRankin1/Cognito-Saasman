import { Component, enableProdMode, OnInit } from '@angular/core';
import { Employee, State, ProjectsService } from './projects.service';
import { confirm } from 'devextreme/ui/dialog';
import DataGrid from 'devextreme/ui/data_grid';
import { AdminService } from '../../admin.service';

if (!/localhost/.test(document.location.host)) {
    enableProdMode();
}

@Component({
    selector: 'app-projects',
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.scss'],
})
export class ProjectsComponent implements OnInit {
    dataSource: Employee[];
    states: State[];
    startEditAction: string = 'dblClick';
    selectTextOnEditStart: boolean = true;
    selectedItemKeys: number[] = [];
    dataGridInstance: DataGrid;

    constructor(private adminService: AdminService, private service: ProjectsService) {
        this.dataSource = service.getEmployees();
    }

    ngOnInit() {
        this.adminService.setTitle('Projects');
    }

    saveGridInstance(e) {
        this.dataGridInstance = e.component;
    }

    contentReady(e) {}

    selectionChanged(e) {
        this.selectedItemKeys = e.selectedRowKeys;
    }

    deleteRecords() {
        let result = confirm('Are you sure you want to delete the selected records?', 'Confirm');
        result.then((dialogResult) => {
            if (dialogResult) {
                this.selectedItemKeys.forEach((key) => {});
                let gridobject: DataGrid = this.dataGridInstance;
                setTimeout(function () {
                    gridobject.refresh();
                }, 1000);
            }
        });
    }
}
