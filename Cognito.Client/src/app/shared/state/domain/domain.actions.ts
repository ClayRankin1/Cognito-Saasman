// eslint-disable-next-line @typescript-eslint/no-namespace
export namespace Domain {
    export class Edit {
        static readonly type = '[Domain] Edit';
        constructor(public payload: { id: number }) {}
    }
}
