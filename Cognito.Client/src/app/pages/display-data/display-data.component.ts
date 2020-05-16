import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: 'display-data.component.html',
})
export class DisplayDataComponent {
    http: HttpClient;
}
