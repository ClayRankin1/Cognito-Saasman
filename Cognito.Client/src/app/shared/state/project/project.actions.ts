// eslint-disable-next-line @typescript-eslint/no-namespace
export namespace Project {
    export class Edit {
        static readonly type = '[Project] Edit';
        constructor(public payload: { id: number }) {}
    }
}
