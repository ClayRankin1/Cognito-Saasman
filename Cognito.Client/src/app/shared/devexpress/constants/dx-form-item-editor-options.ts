export class DxFormItemEditorOption {
    constructor(editorOptions: any) {
        for (const prop in editorOptions) {
            if (Object.prototype.hasOwnProperty.call(editorOptions, prop)) {
                this[prop] = editorOptions[prop];
            }
        }
    }

    extend(additionalOptions: any): any {
        return Object.assign(this, additionalOptions);
    }
}

export class DxMaskedFormItemEditorOption extends DxFormItemEditorOption {
    constructor(editorOptions: any, maskPattern: string) {
        super(editorOptions);
        // eslint-disable-next-line dot-notation
        this['onKeyPress'] = (e): any => {
            // eslint-disable-next-line id-blacklist
            const key = String.fromCharCode(!e.event.charCode ? e.event.which : e.event.charCode);
            const regex = new RegExp(maskPattern);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        };
    }
}

export class DxFormItemEditorOptions {
    public static Integer = new DxFormItemEditorOption({ format: '#0', min: 0, max: 999999999 });
    public static Currency = new DxFormItemEditorOption({
        format: '#,##0.##',
        min: 0,
        max: 999999999,
    });
    public static Percentage = new DxFormItemEditorOption({ format: '#0', min: 0, max: 100 });
    public static Zip = new DxFormItemEditorOption({
        maxLength: 5,
        mask: '00000',
        maskInvalidMessage: 'Zip code is 5 digits',
    });
    public static AlphanumericMask = new DxMaskedFormItemEditorOption({}, '[a-zA-Z0-9]');
}
