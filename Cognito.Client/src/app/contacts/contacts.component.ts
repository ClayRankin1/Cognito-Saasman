import { Component, OnInit, ViewChild } from '@angular/core';
import { environment } from '../../environments/environment';
import { CommonService } from '../shared/services/common.service';
import { ContactsDataService } from './contacts.service';
import { DxDataGridComponent } from 'devextreme-angular';
import { AuthService } from '../auth/auth.service';
import { ITasksGridChanges } from '../shared/services/common.service';
import CustomStore from 'devextreme/data/custom_store';
import { MenuList } from '../shared/models/menulist';
import { ContactRowData } from './contacts.model';
import { CreateRestAspNetStore } from '../shared/data-sources/data-sources';
import DataGrid from 'devextreme/ui/data_grid';
import { confirm, alert } from 'devextreme/ui/dialog';
import { removeMultiRowsInCustomStore } from '../shared/sharedmodule/methods';

@Component({
    selector: 'app-contact',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.css'],
})
export class ContactsComponent implements OnInit {
    @ViewChild('dataGridRef') dataGrid: DxDataGridComponent;
    dataSource: CustomStore;
    clickflag: boolean;
    contactId: number;
    selectedRowsData: ContactRowData[];
    menulist: MenuList[];
    contactsMenu: MenuList[];
    taskId: number;
    dataGridInstance: DataGrid;
    selectedItemKeys: number[] = [];
    namePattern: string = "^[^0-9]+$";
    cityPattern: string = "^[^0-9]+$";
    phonePattern: any = /^\+\s*1\s*\(\s*[02-9]\d{2}\)\s*\d{3}\s*-\s*\d{4}$/;
    phoneRules: any = {
        X: /[02-9]/
    };

    constructor(
        private auth: AuthService,
        private _commService: CommonService,
        private _contactsDataService: ContactsDataService
    ) {
        this.menulist = [
            {
                id: '1',
                name: 'Show All',
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
                name: 'Linking',
                icon: '',
                items: [
                    {
                        id: '4_1',
                        name: 'Link',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '4_2',
                        name: 'UnLink',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '5',
                name: 'Entities',
                icon: '',
                items: [],
            },
            {
                id: '6',
                name: 'Repeat',
                icon: '',
                items: [],
            },
            {
                id: '7',
                name: 'Reports',
                icon: '',
                items: [],
            },
            {
                id: '8',
                name: 'Delete',
                icon: '',
                items: [],
            },
        ];
        _commService.updateListsObs.subscribe((response: ITasksGridChanges) => {
            this.taskId = response.taskId;
            this.getContacts();
        });
    }

    ngOnInit() {
    }
    selectionChanged(e) {
        this.selectedItemKeys = e.selectedRowKeys;

        if (this.selectedItemKeys.length > 1) {
            document
                .getElementById('multi_del_contact_btn')
                .parentElement.classList.remove('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'none';
            });
        } else {
            document
                .getElementById('multi_del_contact_btn')
                .parentElement.classList.add('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'unset';
            });
        }
    }
    contentReady(e) {
    }
    onInitialized(e): void {
        this.dataGridInstance = e.component;
    }
    onInitNewRow(e) { }
    onRowClick(e) {
        this.dataGridInstance.deselectRows(e.data.id);
        e.component.collapseAll(-1);
        if (this.contactId === e.data.id && !this.clickflag) {
            this.clickflag = true;
        } else {
            e.component.expandRow(e.data.id);
            this.clickflag = false;
        }
        this.contactId = e.data.id;
    }
    onCellPrepared(e) {
    }
    getContacts() {
        const auth = this.auth;
        const taskId = this.taskId;
        this.dataSource = CreateRestAspNetStore({
            entityName: 'Contact',
            key: 'id',
            url: environment.activeURL + 'api/contacts',
            loadUrl: () => `${environment.activeURL}api/contacts`,
            loadParams: { taskId: this.taskId },
            authService: auth,
            onBeforeSend(method, ajaxOptions) {
                if (method === 'insert') {
                    let data = JSON.parse(ajaxOptions.data);
                    data.taskId = taskId;
                    ajaxOptions.data = JSON.stringify(data);
                }
            }
        });
    }
    onRowUpdating(e): void {
        e.newData = Object.assign(e.oldData, e.newData);
    }
    MenuClick(data) {
        const item: MenuList = data.itemData;
        switch (item.name) {
            case 'New': {
                this.dataGridInstance.addRow();
                break;
            }
            case 'Edit': {
                if (this.selectedItemKeys.length) {
                    const clickedId: number = this.selectedItemKeys[this.selectedItemKeys.length - 1];
                    const clickedIndex: number = this.dataGridInstance.getRowIndexByKey(clickedId);
                    this.dataGridInstance.editRow(clickedIndex);
                } else {
                    alert('No record selected.', 'Message');
                    return false;
                }
                break;
            }
            case 'Delete': {
                this.deleteRecords(this);
                break;
            }
            default: {
                break;
            }
        }
    }

    deleteRecords(e) {
        removeMultiRowsInCustomStore(this.selectedItemKeys, this.dataSource, this.dataGridInstance);
    }

    onToolbarPreparing = (e) => {
        e.toolbarOptions.items.unshift({
            location: 'before',
            widget: 'dxButton',
            name: 'multiDel',
            options: {
                onClick: this.deleteRecords.bind(this),
                hint: 'Delete',
                disabled: true,
                width: '35x',
                height: '35px',
                template(data, element) {
                    element.classList.add('icon-delete');
                    element.id = 'multi_del_contact_btn';
                },
            },
        });

        let toolbarItems = e.toolbarOptions.items;
        let addRowButton = toolbarItems.find((x) => x.name === 'addRowButton');
        let columnChooserButton = toolbarItems.find((x) => x.name === 'columnChooserButton');
        let searchPanel = toolbarItems.find((x) => x.name === 'searchPanel');
        if (addRowButton) {
            addRowButton.location = 'before';
        }
        if (columnChooserButton) {
            columnChooserButton.location = 'before';
        }
        if (searchPanel) {
            searchPanel.location = 'before';
        }

        let nToolbarItems = [];
        if (toolbarItems.length) {
            nToolbarItems[0] = toolbarItems[3];
            nToolbarItems[1] = toolbarItems[1];
            nToolbarItems[2] = toolbarItems[2];
            nToolbarItems[3] = toolbarItems[0];
        }
        e.toolbarOptions.items = nToolbarItems;
    };
}
