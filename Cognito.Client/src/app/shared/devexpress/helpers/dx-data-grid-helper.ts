import DxDataGrid from 'devextreme/ui/data_grid';
import { DxDataGridOptions } from '../constants/dx-data-grid-options';
import { DxDataGridEvents } from '../constants/dx-data-grid-events';
import * as _ from 'lodash';

export class DxDataGridHelper {
    constructor(private dataGrid: DxDataGrid) {}

    expandRowOnClick(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.RowClick, (e) => {
            if (e.rowType === 'data' && e.handled !== true) {
                const key = e.component.getKeyByRowIndex(e.rowIndex);
                const expanded = e.component.isRowExpanded(key);
                if (expanded) {
                    e.component.collapseRow(key);
                } else {
                    e.component.expandRow(key);
                }
            }
        });
        return this;
    }

    oneRowExpandedAtaTime(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.RowExpanding, (e) => {
            e.component.collapseAll(-1);
        });
        return this;
    }

    sendAllFieldsOnUpdate(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.RowUpdating, (e) => {
            e.newData = _.merge({}, e.oldData, e.newData);
        });
        return this;
    }

    setPopupTitle(titleWhenAddingNewRecord, titleWhenUpdatingRecord): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.InitNewRow, (e) => {
            e.component.option(DxDataGridOptions.PopupTitle, titleWhenAddingNewRecord);
        });

        this.dataGrid.on(DxDataGridEvents.EditingStart, (e) => {
            e.component.option(DxDataGridOptions.PopupTitle, titleWhenUpdatingRecord);
        });
        return this;
    }

    hideToolbar(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.ToolbarPreparing, (e) => {
            e.toolbarOptions.visible = false;
        });
        return this;
    }

    setToolbarAddRecordButtonHint(hint): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.ToolbarPreparing, (e) => {
            const addRowButtonOption = e.toolbarOptions.items.find(
                (item) => item.name === 'addRowButton'
            );
            if (addRowButtonOption) {
                addRowButtonOption.options.hint = hint;
            }
        });
        return this;
    }

    fillMasterDataObject(detailProperty: string, masterRow): DxDataGridHelper {
        const fillMasterData = (e): void => {
            e.data[detailProperty] = masterRow.data;
        };
        this.dataGrid.on(DxDataGridEvents.InitNewRow, fillMasterData);
        this.dataGrid.on(DxDataGridEvents.EditingStart, fillMasterData);

        return this;
    }

    fillMasterKey(detailProperty: string, masterRow): DxDataGridHelper {
        const fillMasterKey = (e): void => {
            e.data[detailProperty] = masterRow.key;
        };
        this.dataGrid.on(DxDataGridEvents.InitNewRow, fillMasterKey);
        this.dataGrid.on(DxDataGridEvents.EditingStart, fillMasterKey);

        return this;
    }

    // When a Devexpress textbox is empty, it sends an empty string to the server instead of null
    // This method intends to fix that
    replaceEmptyStringWithNull(dataField: string): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.EditorPreparing, (e) => {
            if (e.parentType === 'dataRow' && e.dataField === dataField) {
                e.editorOptions.onValueChanged = (args: any): void => {
                    if (args.value === '') {
                        args.value = null;
                    }
                    e.component.cellValue(e.row.rowIndex, dataField, args.value);
                };
            }
        });

        return this;
    }

    // Navigates to the page where a new record has been added or updated
    applyRowNavigationBehavior(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.RowInserted, (e) => {
            e.component.navigateToRow(e.key);
        });
        this.dataGrid.on(DxDataGridEvents.RowUpdated, (e) => {
            e.component.navigateToRow(e.key);
        });
        return this;
    }

    // When changing pages on the grid, the grid height is not recalculated to fit only the records in that page
    // This method forces a resize to fix that
    resizeOnContentReady(): DxDataGridHelper {
        this.dataGrid.on(DxDataGridEvents.ContentReady, (e) => {
            e.component.resize();
        });

        return this;
    }
}
