import { Component, enableProdMode, OnInit } from '@angular/core';
import { Service, Link } from './links.service';
import { confirm } from 'devextreme/ui/dialog';
import DataGrid from 'devextreme/ui/data_grid';

if (!/localhost/.test(document.location.host)) {
    enableProdMode();
}

@Component({
    selector: 'app-links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.scss'],
    providers: [Service],
})
export class LinksComponent implements OnInit {
    dataSource: Link[];
    startEditAction: string = 'dblClick';
    selectTextOnEditStart: boolean = true;
    selectedItemKeys: number[] = [];
    dataGridInstance: DataGrid;

    constructor(service: Service) {
        this.dataSource = service.getLinks();
    }

    ngOnInit() {}

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

    clear() {}
}
