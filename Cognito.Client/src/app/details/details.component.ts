import { Component, Input } from '@angular/core';
import { CommonService } from '../shared/services/common.service';
import { environment } from '../../environments/environment';
import { AuthService } from '../auth/auth.service';
import CustomStore from 'devextreme/data/custom_store';
import { HttpClient } from '@angular/common/http';
import { alert } from 'devextreme/ui/dialog';
import DataGrid from 'devextreme/ui/data_grid';
import { DetailsService } from './details.service';
import { combineLatest, Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { byTextAscending, getLookupNameById, removeMultiRowsInCustomStore } from '../shared/sharedmodule/methods';
import { DetailRowData } from './details.model';
import { MenuList } from '../shared/models/menulist';
import { CognitoTimePipe } from '../shared/sharedmodule/datepipe.module';
import { LookupService } from '../data/service/lookup.service';
import { LookupType, DetailTypeLookup, Lookup } from '../data/schema/lookup';
import { ProjectService, DetailService } from '../data/service';
import { CreateRestAspNetStore } from '../shared/data-sources/data-sources';
import { Observable } from 'rxjs';

enum SourceType {
    User = 'user',
    System = 'system',
}

interface DetaulType {
    detailTypeName: string;
}

interface detailType {
    id: number;
    detailTypeName: string;
}

@Component({
    selector: 'app-details',
    templateUrl: './details.component.html',
    styleUrls: ['./details.component.css'],
})
export class DetailsComponent {
    private readonly nonSplittableItems = [
        'Doc Reference',
        'Contact Reference',
        'Quote',
        'Web Reference',
        'Email',
    ];
    private gridClearEventSubscription: Subscription;
    dataSource: CustomStore;
    url: string;
    projects: Lookup[];
    detailTypeLookups: DetailTypeLookup[];
    detailTypesDataSource: DataSource;
    projectName: string;
    clickflag: boolean;
    selectedItemKeys: number[] = [];
    dataGridInstance: DataGrid;
    clickedRowIndex: number;
    editedDetail: DetailRowData;
    detail: string;
    detailValue: string;
    checkedValue: boolean;
    clickCount: number;
    clickedRowID: number;
    id: number;
    filterClickedCount: number;
    filterVisible: boolean;
    showLinked = false;
    storeData: DetailRowData[];
    sourceType = SourceType;
    menulist: MenuList[];
    cognitoTimePipe = new CognitoTimePipe();
    @Input() gridClearEvent: Observable<void>;

    ngOnInit() {
        this.gridClearEventSubscription = this.gridClearEvent.subscribe(() => this.clearGrid());
    }

    constructor(
        private _commService: CommonService,
        private http: HttpClient,
        private auth: AuthService,
        private details: DetailsService,
        private lookupService: LookupService,
        private projectService: ProjectService,
        private detailService: DetailService
    ) {
        this.checkedValue = false;
        this.clickCount = 0;
        this.filterClickedCount = 0;
        this.filterVisible = false;
        this.menulist = [
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
                name: 'Copy',
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
                        name: 'Toggle Filter Row',
                        icon: 'isblank',
                        items: [],
                    },
                    {
                        id: '4_2',
                        name: 'Lines',
                        icon: '',
                        items: [
                            {
                                id: '4_2_1',
                                name: 'One Line',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_2_2',
                                name: 'Five Lines',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_2_3',
                                name: 'Ten Lines',
                                icon: '',
                                items: [],
                            },
                            {
                                id: '4_2_4',
                                name: 'All',
                                icon: '',
                                items: [],
                            },
                        ],
                    },
                    {
                        id: '4_3',
                        name: 'Show Linked Info',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '5',
                name: 'Split',
                icon: '',
                items: [],
            },
            {
                id: '6',
                name: 'Merge',
                icon: '',
                items: [],
            },
            {
                id: '7',
                name: 'Report',
                icon: '',
                items: [
                    {
                        id: '7_1',
                        name: 'Items Detail Report',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '8',
                name: 'Export',
                icon: '',
                items: [
                    {
                        id: '8_1',
                        name: 'Email Last Export',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_2',
                        name: 'RTF',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_3',
                        name: 'CSV',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_4',
                        name: 'PDF',
                        icon: '',
                        items: [],
                    },
                    {
                        id: '8_5',
                        name: 'XLS',
                        icon: '',
                        items: [],
                    },
                ],
            },
            {
                id: '9',
                name: 'Delete',
                icon: '',
                items: [],
            },
        ];
        this._commService.updateListsObs.subscribe((response) => {
            this.getDetails(response);
        });

        this.details.showLinked$.subscribe((showLinked: boolean) => {
            this.showLinked = showLinked;
        });

        // todo: FIXME - Refactor ProjectId in `ProjectLookup` to Id
        this.projectService.getAll().subscribe((projects) => {
            const projectsLookup = new DataSource({
                store: new ArrayStore({
                    data: projects.map((p) => ({
                        id: p.id,
                        label: p.nickname,
                    })),
                }),
                paginate: false,
            });
            projectsLookup.load();
            this.projects = projectsLookup.items();
        });

        combineLatest(
            this.lookupService.getLookup<Lookup>(LookupType.DetailTypeSourceType),
            this.lookupService.getLookup<DetailTypeLookup>(LookupType.DetailType)
        ).subscribe(([sourceTypes, detailTypes]) => {
            const userSourceType = sourceTypes.find((st) => st.label === 'User');
            this.detailTypeLookups = detailTypes;
            this.detailTypesDataSource = new DataSource({
                store: new ArrayStore({
                    data: this.detailTypeLookups.filter((i) => i.detailTypeSourceTypeId === userSourceType.id),
                }),
            });
            this.detailTypesDataSource.load();
        });

        this.onSave = this.onSave.bind(this);
        this.onSplit = this.onSplit.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.onDetailValueChanged = this.onDetailValueChanged.bind(this);
    }

    getDetails(response): void {
        const auth = this.auth;
        const _taskId: number = response.taskId;
        const _detailTypeId: number = response.detailTypeId;
        this.url = environment.activeURL;
        if (!isNaN(_taskId)) {
            this.dataSource = CreateRestAspNetStore({
                entityName: 'Detail',
                key: 'id',
                url: environment.activeURL + 'api/Details',
                loadUrl: () => `${environment.activeURL}api/Tasks/${_taskId}/details`,
                loadParams: { detailTypeId: _detailTypeId },
                onLoaded: (result: any[]) => {
                    this.storeData = result as DetailRowData[];
                },
                authService: auth,
            });
        }
    }

    onInitNewRow(e) {
        this.dataGridInstance.option('editing.popup.title', 'Add Detail');

        e.data.taskId = localStorage.getItem('taskId');
        e.data.projectId = localStorage.getItem('projectId');

        // Set the type field to "Note"
        const detailType = this.detailTypeLookups.find((item) => item.label === 'Note');
        e.data.detailTypeId = detailType.id;
    }
    onRowClick(e) {
        if (e.rowType === 'data') {
            this.dataGridInstance.deselectRows(e.data.id);
            if (this.clickCount < 2) { this.clickCount++; }
            else { this.clickCount = 1; }

            setTimeout(() => {
                if (this.clickCount > 1) {
                    e.component.collapseAll(-1);
                    if (this.clickedRowID == e.data.id && !this.clickflag) {
                        this.clickflag = true;
                    } else {
                        e.component.expandRow(e.data.id);
                        this.clickflag = false;
                    }
                    this.clickedRowID = e.data.id;
                } else {
                    if (this.clickCount > 0) {
                        if (e.data.id != this.id) {
                            const pRowIndex: number = e.component.getRowIndexByKey(
                                +localStorage.getItem('detailId')
                            );
                            e.component
                                .getRowElement(pRowIndex)[0]
                                .classList.remove('dx-double-click');
                            e.component
                                .getRowElement(e.rowIndex)[0]
                                .classList.add('dx-double-click');
                            e.event.preventDefault();
                            localStorage.setItem('detailId', e.data.id);
                            this.id = e.data.id;
                        }
                    }
                }
                this.clickCount = 0;
            }, 300);
        }
    }
    selectionChanged(e) {
        this.details.updateSelectedItems(e.selectedRowKeys);
        this.selectedItemKeys = e.selectedRowKeys;
        this.clickedRowIndex = this.dataGridInstance.getRowIndexByKey(e.selectedRowKeys[0]);

        if (this.selectedItemKeys.length > 1) {
            document
                .getElementById('multi_del_item_btn')
                .parentElement.classList.remove('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'none';
            });
        } else {
            document
                .getElementById('multi_del_item_btn')
                .parentElement.classList.add('dx-state-disabled');
            e.element.querySelectorAll('.dx-link-delete').forEach((el) => {
                el.style.display = 'unset';
            });
        }
    }
    contentReady(e) {
        if (+localStorage.getItem('detailId')) {
            if (e.component.getRowIndexByKey(+localStorage.getItem('detailId')) >= 0) {
                e.component
                    .getRowElement(
                        e.component.getRowIndexByKey(+localStorage.getItem('detailId'))
                    )[0]
                    .classList.add('dx-double-click');
            }
        }
    }

    onInitialized(e): void {
        this.dataGridInstance = e.component;
    }

    onEditorPreparing(e): void {
        if (e.parentType === 'dataRow' && e.dataField === 'chrono') {
            e.editorOptions.openOnFieldClick = true;
            e.editorOptions.onOpened = function (args) {
                const value = args.component._options.value;
                if (!value || (value.getHours() === 0 && value.getMinutes() === 0)) {
                    let date = new Date();
                    date.setHours(0, 0, 0, 0);
                    args.component._strategy._timeView.option('value', date);
                    args.component._strategy._widget.option('value', date);
                }
            }
        }

        if (e.parentType === 'filterRow' && e.dataField === 'detailTypeId') {
            e.editorOptions.dataSource = this.getAvailableTypeLookup();
        }
    }

    onRowUpdating(e): void {
        e.newData = Object.assign(e.oldData, e.newData);
    }

    onRowUpdated(e): void {
        this.dataGridInstance.deselectAll();
    }

    getProjectNamebyID(projectId: number): string {
        return getLookupNameById(this.projects, projectId);
    }

    getTypeNameById(typeId: number): string {
        return getLookupNameById(this.detailTypeLookups, typeId);
    }

    onToolbarPreparing(e) {
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
                    element.id = 'multi_del_item_btn';
                },
            },
        });

        const toolbarItems = e.toolbarOptions.items;
        const addRowButton = toolbarItems.find((x) => x.name === 'addRowButton');
        const columnChooserButton = toolbarItems.find((x) => x.name === 'columnChooserButton');
        const searchPanel = toolbarItems.find((x) => x.name === 'searchPanel');
        if (addRowButton) {
            addRowButton.location = 'before';
        }
        if (columnChooserButton) {
            columnChooserButton.location = 'before';
        }
        if (searchPanel) {
            searchPanel.location = 'before';
        }

        const nToolbarItems = [];
        if (toolbarItems.length) {
            nToolbarItems[0] = toolbarItems[3];
            nToolbarItems[1] = toolbarItems[1];
            nToolbarItems[2] = toolbarItems[2];
            nToolbarItems[3] = toolbarItems[0];
        }
        e.toolbarOptions.items = nToolbarItems;
    }

    deleteRecords(e) {
        removeMultiRowsInCustomStore(this.selectedItemKeys, this.dataSource, this.dataGridInstance);
    }
    MenuClick = (data) => {
        const item: MenuList = data.itemData;
        switch (item.name) {
            case 'New': {
                this.toggleSplitButton(false);
                this.dataGridInstance.addRow();
                break;
            }
            case 'Edit': {
                if (this.selectedItemKeys.length) {
                    const clickedId: number = this.selectedItemKeys[this.selectedItemKeys.length - 1];
                    const clickedIndex: number = this.dataGridInstance.getRowIndexByKey(clickedId);
                    this.toggleSplitButton(false);
                    this.dataGridInstance.editRow(clickedIndex);
                } else {
                    alert('No record selected.', 'Message');
                    return false;
                }
                break;
            }
            case 'Split': {
                if (this.selectedItemKeys.length !== 1) {
                    alert('Select one row for splitting only.', 'One row only');
                    return;
                }

                const selectedRowsData: DetailRowData[] = this.dataGridInstance.getSelectedRowsData();
                const rowToSplit: DetailRowData = selectedRowsData[0];
                const detailTypeName: string = this.getTypeNameById(rowToSplit.detailTypeId);

                if (this.nonSplittableItems.includes(detailTypeName)) {
                    alert(`${detailTypeName}s cannot be split.`, 'Cannot be split');
                    return;
                }
                if (rowToSplit.detail.includes('^')) {
                    this.splitRows(rowToSplit);
                } else {
                    this.toggleSplitButton(true);
                    this.dataGridInstance.editRow(this.clickedRowIndex);
                }
                break;
            }
            case 'Merge': {
                if (!this.selectedItemKeys || !this.selectedItemKeys.length) {
                    alert('Select some rows to be merged.', 'No rows selected');
                    return;
                }

                this.http
                    .put(this.url + 'api/Details/Merge', { detailIds: this.selectedItemKeys })
                    .pipe(
                        tap(() => {
                            this.dataGridInstance.beginCustomLoading(
                                'Merging the selected Details.'
                            );
                        })
                    )
                    .subscribe(() => {
                        setTimeout(() => {
                            this.dataGridInstance.endCustomLoading();
                            this.dataGridInstance.refresh();
                        }, 1000);
                    });
                break;
            }
            case 'Delete': {
                this.deleteRecords(this);
                break;
            }
            case 'Toggle Filter Row': {
                this.filterClickedCount++;
                this.filterVisible = this.filterClickedCount % 2 == 0 ? false : true;
                if (this.filterVisible) {
                    item.icon = 'check';
                } else {
                    item.icon = 'isblank';
                }
                break;
            }
            case 'Show Linked Info':
            case 'Show All': {
                this.details.toggleShowLinked();
                this.menulist[3].items[2].name = this.showLinked ? 'Show All' : 'Show Linked Info';
                break;
            }
            default: {
                break;
            }
        }
    };

    private toggleSplitButton(isVisible: boolean) {
        this.dataGridInstance.option('editing.popup.toolbarItems[1].options.visible', isVisible);
    }

    private splitRows(rowToSplit: { id: number }) {
        if (!rowToSplit) {
            alert('Select one row for splitting first.', 'No row is selected');
            return;
        }

        this.http
            .put(this.url + 'api/Details/Split', { detailIds: [rowToSplit.id] })
            .pipe(
                tap(() => {
                    this.dataGridInstance.beginCustomLoading('Splitting the selected Details.');
                })
            )
            .subscribe(() => {
                setTimeout(() => {
                    this.dataGridInstance.endCustomLoading();
                    this.dataGridInstance.refresh();
                }, 1000);
            });
    }

    onSave(e) {
        this.dataGridInstance.saveEditData();
    }

    onSplit(e) {
        const detail_value: string = this.detailValue ?? this.detail;
        if (detail_value.includes('^')) {
            this.dataGridInstance.saveEditData().then(() => {
                this.splitRows(this.editedDetail);
            });
        } else {
            alert(
                'You must include the caret character \'^\' in the Detail field at locations where you want to \'split\' this record.',
                'Split Warning'
            );
        }
    }
    onCancel(e) {
        this.dataGridInstance.cancelEditData();
    }
    editstart(e) {
        this.dataGridInstance.option('editing.popup.title', 'Edit Detail');
        this.editedDetail = e.data;
        this.detail = e.data.detail;
        this.detailValue = this.detail;
        this.checkedValue = false;
    }
    onDetailValueChanged(e) {
        this.detailValue = e.component._changedValue;
    }

    getAvailableTypeLookup(): detailType[] {
        if (this.storeData.length > 0) {
            const typeArray: number[] = [];
            let newArray: detailType[] = this.storeData.map((item) => {
                if (!typeArray.includes(item.detailTypeId) && item.detailTypeId) {
                    const value = {
                        id: item.detailTypeId,
                        detailTypeName: this.getTypeNameById(item.detailTypeId),
                    };
                    typeArray.push(item.detailTypeId);
                    return value;
                }
            });
            newArray = newArray.filter((item) => !!item);
            newArray.sort(byTextAscending((type: DetaulType) => type.detailTypeName));
            return newArray;
        }
    }

    setChrono = (cellInfo: DetailRowData): string => {
        if (cellInfo.chrono) {
            const chrono: Date = new Date(cellInfo.chrono);
            if (chrono.getHours() === 0 && chrono.getMinutes() === 0) {
                return this.cognitoTimePipe.transform(chrono, 'shortDate', '');
            } else {
                return this.cognitoTimePipe.transform(chrono, 'short', '');
            }
        }
    };

    clearGrid() {
        this.dataGridInstance.option('dataSource', []);
    }
}
