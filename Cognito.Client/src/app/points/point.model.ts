export class Point {
    constructor(
        public id: number,
        public dateUpdated: Date,
        public dateAdded: Date,
        public matter: any,
        public projectId: number,
        public text: string,
        public parentId: number | null,
        public parent: any,
        public count: number,
        public children: Point[],
        public label: string
    ) {}
}

export class InsertedPoint {
    parentId: number | null;
    text: string;
}
