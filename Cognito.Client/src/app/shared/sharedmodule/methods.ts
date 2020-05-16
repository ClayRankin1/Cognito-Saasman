import CustomStore from 'devextreme/data/custom_store';
import DataGrid from 'devextreme/ui/data_grid';
import { confirm, alert } from 'devextreme/ui/dialog';

export const byTextAscending = <T>(getTextProperty: (object: T) => String) => (
    objectA: T,
    objectB: T
) => {
    const upperA = getTextProperty(objectA).toUpperCase();
    const upperB = getTextProperty(objectB).toUpperCase();
    if (upperA < upperB) {
        return -1;
    }
    if (upperA > upperB) {
        return 1;
    }
    return 0;
};

export const getLookupNameById = (
    lookup: any,
    id: number,
    idPropertyName: string = 'id',
    namePropertyName = 'label'
): string => {
    let element: any = lookup.find((x) => x[idPropertyName] === id);
    if (element) {
        return element[namePropertyName];
    } else {
        return '';
    }
};

export const getLookupIdByName = (
    lookup: any,
    name: string,
    namePropertyName: string = 'label',
    idPropertyName: string = 'id'
): number => {
    let element: any = lookup.find((x) => x[namePropertyName] === name);
    if (element) {
        return element[idPropertyName];
    } else {
        return;
    }
};

export const removeMultiRowsInCustomStore = (
    selectedRowKeys: number[], dataSource: CustomStore, dataGridInstance: DataGrid, confirmMessage: string = 'Are you sure you want to delete the selected records?'
): void => {
    if (selectedRowKeys.length) {
        const result = confirm(confirmMessage, 'Confirm');
        result.then((dialogResult: boolean) => {
            if (dialogResult) {
                let removedRowCount: number = 0;
                selectedRowKeys.forEach((key) => {
                    dataSource.remove(key).then(
                        (key) => {
                            removedRowCount++;
                            if (removedRowCount === selectedRowKeys.length) {
                                dataGridInstance.refresh();
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
