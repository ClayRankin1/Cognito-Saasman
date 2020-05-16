import { PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

export class CognitoTimePipe implements PipeTransform {
    transform(
        value: number | string | Date,
        format: string = 'yyyy-MM-dd',
        timezone: string = 'UTC'
    ): string {
        return new DatePipe('en-US').transform(value, format, timezone);
    }
}
