import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponseBase, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';
import { User } from '../auth/user.model';
import notify from 'devextreme/ui/notify';
import { BehaviorSubject, Observable } from 'rxjs';
import { Point } from './point.model';
import { InsertedPoint } from './point.model';

@Injectable({
    providedIn: 'root',
})
export class PointsService {
    private currentUser: User;
    private points: BehaviorSubject<Point[]>;
    public selectedPoints: Observable<Point[]>;

    constructor(private http: HttpClient, private auth: AuthService) {
        this.currentUser = this.auth.currentUser;
        this.points = new BehaviorSubject([]);
        this.selectedPoints = this.points.asObservable();
    }

    updateSelectedPoints(points: Point[]) {
        this.points.next(points);
    }

    getPoints = async () => {
        const points = await this.http
            .get(environment.activeURL + 'api/points/project', {
                params: {
                    projectId: localStorage.getItem('projectId') || '0',
                },
                headers: { Authorization: `Bearer ${this.currentUser.accessToken}` },
                observe: 'response',
            })
            .toPromise()
            .then((res: HttpResponse<any>) => {
                return {
                    data: res.body,
                    totalCount: res.body.length,
                };
            })
            .catch((e) => {
                throw 'Data Loading Error';
            });

        return points;
    };

    getLinkedPoints = async (itemIds: number[]) => {
        const points = await this.http
            .get(environment.activeURL + 'api/points/linked', {
                params: {
                    items: `${itemIds}`,
                },
                headers: { Authorization: `Bearer ${this.currentUser.accessToken}` },
                observe: 'response',
            })
            .toPromise()
            .then((res: HttpResponse<any>) => {
                return {
                    data: res.body,
                    totalCount: res.body.length,
                };
            })
            .catch((e) => {
                throw 'Data Loading Error';
            });

        return points;
    };

    insertPoint = async (values: InsertedPoint) => {
        if (values.parentId === 0) {
            values.parentId = null;
        }
        const point = await this.http
            .post(
                environment.activeURL + 'api/points',
                {
                    ...values,
                    projectId: localStorage.getItem('projectId') || '0',
                },
                {
                    headers: { Authorization: `Bearer ${this.currentUser.accessToken}` },
                    observe: 'response',
                }
            )
            .toPromise()
            .then((res: HttpResponse<any>) => {})
            .catch(() => {
                throw 'Data Loading Error';
            });

        return point;
    };

    deletePoint = async (key: number) => {
        return await this.http
            .delete(environment.activeURL + `api/points/${key}`, {
                headers: {
                    Authorization: `Bearer ${this.currentUser.accessToken}`,
                },
                observe: 'response',
            })
            .toPromise()
            .then((res: HttpResponse<any>) => {})
            .catch(() => {
                throw 'Data Removal Error';
            });
    };

    updatePoint = async (key: number, point: Point) => {
        return await this.http
            .put(environment.activeURL + `api/points/${key}`, { ...point }, { observe: 'response' })
            .toPromise()
            .then((res: HttpResponse<any>) => {})
            .catch(() => {
                throw 'Data Loading Error';
            });
    };

    reorderPoint = async (key: number, point: Point) => {
        return await this.http
            .post(
                environment.activeURL + `api/points/${key}/reorder`,
                {
                    count: point.count,
                    parentId: point.parentId,
                },
                {
                    observe: 'response',
                }
            )
            .toPromise()
            .then((res: HttpResponse<any>) => {})
            .catch(() => {
                throw 'Data Loading Error';
            });
    };

    linkPoints = async (key: number, itemIds: number[]) => {
        return await this.http
            .post(environment.activeURL + `api/points/${key}/link`, itemIds, {
                observe: 'response',
            })
            .toPromise()
            .catch(() => {
                notify('Points have already been linked.', 'info', 2000);
            });
    };
}
