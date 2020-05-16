export interface GridRowEvent<T> {
    data: T; // This is populated on RowInserting event
    newData: T; // This is populated on RowUpdating event
    oldData: T; // This is populated on RowUpdating event
    key: number;
}
