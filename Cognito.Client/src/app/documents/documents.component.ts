import { Component, ViewChild } from '@angular/core';
import CustomStore from 'devextreme/data/custom_store';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';
import { HttpClient, HttpParams, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonService } from '../shared/services/common.service';
import { DxDataGridComponent } from 'devextreme-angular';
import { confirm, alert } from 'devextreme/ui/dialog';
import { DxDrawerComponent } from 'devextreme-angular';
import { StatusLookup, DocumentRowData } from './documents.model';
import { MenuList } from '../shared/models/menulist';
import { from } from 'rxjs';

@Component({
    selector: 'app-documents',
    templateUrl: './documents.component.html',
    styleUrls: ['./documents.component.scss'],
})
export class DocumentsComponent {
    @ViewChild('clientGrid', { static: true }) clientGrid: DxDataGridComponent;
    @ViewChild(DxDrawerComponent) drawer: DxDrawerComponent;
    dataSource: CustomStore;
    statusLookup: StatusLookup[];
    actId: number;
    docId: number = 0;
    clickflag: boolean;
    isOpened: Boolean = false;
    menulist: MenuList[];

    constructor(
        private auth: AuthService,
        private httpClient: HttpClient,
        private router: Router,
        private _commService: CommonService
    ) {
        const currentUser = this.auth.currentUser;
        this.statusLookup = [
            { ID: 1, Name: 'Draft' },
            { ID: 2, Name: 'Active' },
            { ID: 3, Name: 'Archived' },
        ];
        this.clickflag = false;
        this.menulist = [
            {
                id: '1',
                name: 'Upload',
                icon: '',
                items: [],
            },
            {
                id: '2',
                name: 'New',
                icon: '',
                items: [],
            },
            {
                id: '3',
                name: 'Edit',
                icon: '',
                items: [],
            },
            {
                id: '4',
                name: 'View',
                icon: '',
                items: [
                    {
                        id: '4_1',
                        name: 'Columns',
                        icon: '',
                        items: [
                            {
                                id: '4_1_1',
                                name: 'Maximum',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_1_2',
                                name: 'Standard',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_1_3',
                                name: 'Minimum',
                                icon: '',
                                items: [],
                            },
                        ],
                    },
                    {
                        id: '4_2',
                        name: 'Edit Mode',
                        icon: '',
                        items: [
                            {
                                id: '4_2_1',
                                name: 'PopUp',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_2_2',
                                name: 'Batch',
                                icon: '',
                                items: [],
                            },
                        ],
                    },
                ],
            },
            {
                id: '5',
                name: 'Links',
                icon: '',
                items: [
                    {
                        id: '5_1',
                        name: 'Link',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '5_2',
                        name: 'UnLink',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '6',
                name: 'Scope',
                icon: '',
                items: [
                    {
                        id: '6_1',
                        name: 'Activity',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '6_2',
                        name: 'Event',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '6_3',
                        name: 'Matter',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '6_4',
                        name: 'All',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '7',
                name: 'Export',
                icon: '',
                items: [],
            },
            {
                id: '8',
                name: 'Delete',
                icon: '',
                items: [],
            },
            {
                id: '9',
                name: 'Report',
                icon: '',
                items: [],
            },
            {
                id: '10',
                name: 'Close',
                icon: '',
                items: [],
            },
        ];

        this.dataSource = new CustomStore({
            key: 'id',
            remove: async function (key: number) {
                return (
                    httpClient
                        // todo: FIXME - Do we need it when we have interceptor for this?
                        .delete(environment.activeURL + `api/documents/${key}`, {
                            headers: {
                                Authorization: `Bearer ${currentUser.accessToken}`,
                            },
                            observe: 'response',
                        })
                        .toPromise()
                        .then(() => {})
                        .catch((error) => {
                            throw 'Data Removal Error';
                        })
                );
            },
            update: async function (key: number, value: DocumentRowData) {
                return httpClient
                    .patch(
                        environment.activeURL + `api/documents/${key}`,
                        {
                            ...value,
                        },
                        {
                            headers: {
                                Authorization: `Bearer ${currentUser.accessToken}`,
                            },
                            observe: 'response',
                        }
                    )
                    .toPromise()
                    .then(() => {})
                    .catch((error) => {
                        throw 'Data Removal Error';
                    });
            },
            load: async function () {
                let params: HttpParams = new HttpParams();
                let actId: string =
                    localStorage.getItem('actId') === 'NaN' ? '0' : localStorage.getItem('actId');
                let pageNumber: string = '1';

                params = params.set('ActId', actId);
                params = params.set('pageNumber', pageNumber);
                return httpClient
                    .get(environment.activeURL + 'api/documents', {
                        params,
                        headers: { Authorization: `Bearer ${currentUser.accessToken}` },
                        observe: 'response',
                    })
                    .toPromise()
                    .then((res) => {
                        return {
                            data: res.body,
                            totalCount: JSON.parse(res.headers.get('pagination'))?.totalItems ?? 0,
                        };
                    })
                    .catch((error) => {
                        throw 'Data Loading Error';
                    });
            },
        });

        _commService.updateListsObs.subscribe((response) => {
            let i: string = response.taskId.toString();
            if (this.clientGrid.instance) this.clientGrid.instance.refresh();
        });
    }

    onRowClick(e) {
        if (e.rowType === 'data') {
            if (this.docId == e.data.id && !this.clickflag) {
                e.component.collapseAll(-1);
                this.clickflag = true;
            } else {
                e.component.expandRow(e.data.id);
                this.clickflag = false;
            }
            this.docId = e.data.id;
        }
    }

    selectionChanged(e) {
        e.component.collapseAll(-1);
        e.component.expandRow(e.currentSelectedRowKeys[0]);
    }

    redirecToExtraction = (e) => {
        if (e.row.data.status == 'Draft') {
            var docURL = e.row.data.url;
            var fileId = e.row.data.id;
            localStorage.setItem('selectedDocument', docURL);
            this.router.navigate(['/texteditor', { id: docURL, fileId: fileId }]);
            return;
        }

        var docURL = e.row.data.url;
        var docname = e.row.data.fileName;
        localStorage.setItem('selectedDocument', docURL);
        localStorage.setItem('selectedDocumentName', docname);
        e.event.preventDefault();
        this.router.navigate(['/documentextractor']);
    };

    onToolbarPreparing = (e) => {
        let router: Router = this.router;
        var toolbarItems = e.toolbarOptions.items;
        toolbarItems.forEach((item) => {
            if (item.name === 'addRowButton') {
                item.options.onClick = function (e) {
                    router.navigate(['/texteditor'], {});
                };
            }
        });

        let addRowButton = e.toolbarOptions.items.find((x) => x.name === 'addRowButton');
        let columnChooserButton = e.toolbarOptions.items.find(
            (x) => x.name === 'columnChooserButton'
        );
        let searchPanel = e.toolbarOptions.items.find((x) => x.name === 'searchPanel');
        if (addRowButton) {
            addRowButton.location = 'before';
        }
        if (columnChooserButton) {
            columnChooserButton.location = 'before';
        }
        if (searchPanel) {
            searchPanel.location = 'before';
        }
    };

    MenuClick(data) {
        let item = data.itemData;

        switch (item.name) {
            case 'Upload': {
                this.drawer.instance.toggle();
                break;
            }
            default: {
                break;
            }
        }
    }
}