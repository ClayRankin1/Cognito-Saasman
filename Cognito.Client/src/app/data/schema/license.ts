export interface License {
    id: number;
    price: number;
    licenseTypeId: LicenseType;
}

export enum LicenseType {
    Basic = 1,
    TimeKeeper = 2,
}
