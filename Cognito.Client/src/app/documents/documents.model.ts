export interface StatusLookup {
    ID: number;
    Name: string;
}

export interface DocumentRowData {
    id: number;
    dateAdded: string;
    dateUpdated: string;
    key: String;
    fileName: string;
    description: string;
    status: string;
    url: string;
}
