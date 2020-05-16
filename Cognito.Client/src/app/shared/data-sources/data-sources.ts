/* eslint-disable no-debugger */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable prefer-arrow/prefer-arrow-functions */
import * as AspNetData from 'devextreme-aspnet-data-nojquery';
import CustomStore from 'devextreme/data/custom_store';
import notify from 'devextreme/ui/notify';
import { ApiService } from 'src/app/data/service/api.service';
import DataSource from 'devextreme/data/data_source';
import { AuthService } from '../../auth/auth.service';
import { Observable } from 'rxjs';

export interface AjaxOptions {
    cache?: boolean;
    contentType?: any;
    data?: any;
    dataType?: string;
    headers?: { [key: string]: any };
    method?: string;
    password?: string;
    timeout?: number;
    url?: string;
    username?: string;
    xhrFields?: { [key: string]: any };
}

export interface AspNetStoreConfiguration {
    key: string;
    url: string;
    entityName: string;
    loadUrlSuffix?: string;
    loadParams?: any;
    loadUrl?: () => string;
    authService?: AuthService;
    onBeforeSend?: (operation: string, ajaxSettings: AjaxOptions) => void;
    onLoaded?: (value: any[]) => any;
}

const showSuccessMessage = (message: string): void => {
    // todo: FIXME some global configuration for timeouts...
    notify(message, 'success', 2000);
};

export function CreateRestAspNetStore(configuration: AspNetStoreConfiguration): CustomStore {
    const loadUrl =
        configuration.url + (configuration.loadUrlSuffix ? configuration.loadUrlSuffix : '');

    return AspNetData.createStore({
        key: configuration.key,
        loadUrl,
        insertUrl: configuration.url,
        updateUrl: configuration.url,
        deleteUrl: configuration.url,
        loadParams: configuration.loadParams,
        onBeforeSend: (method, ajaxOptions) => {
            ajaxOptions.xhrFields = { withCredentials: false };
            if (!ajaxOptions.headers) {
                ajaxOptions.headers = {};
            }
            if (method === 'load' && configuration.loadUrl) {
                ajaxOptions.url = `${configuration.loadUrl()}`;
            }
            if (method === 'insert') {
                ajaxOptions.headers['Content-Type'] = 'application/json';
                ajaxOptions.data = ajaxOptions.data.values;
            }
            if (method === 'update') {
                ajaxOptions.headers['Content-Type'] = 'application/json';
                ajaxOptions.url = `${ajaxOptions.url}/${ajaxOptions.data.key}`;
                ajaxOptions.data = ajaxOptions.data.values;
            }
            if (method === 'delete') {
                ajaxOptions.url = `${ajaxOptions.url}/${ajaxOptions.data.key}`;
            }

            if (configuration?.authService?.currentUser?.accessToken) {
                ajaxOptions.headers.Authorization = `Bearer ${configuration.authService.currentUser.accessToken}`;
            }
            ajaxOptions.xhrFields = { withCredentials: false };

            if (configuration.onBeforeSend) {
                configuration.onBeforeSend(method, ajaxOptions);
            }
        },
        onInserted: (values: any, key: any) => {
            showSuccessMessage(`${configuration.entityName} has been created successfully.`);
        },
        onUpdated: (key: any, values: any) => {
            showSuccessMessage(`${configuration.entityName} has been updated successfully.`);
        },
        onRemoved: (key: any) => {
            showSuccessMessage(`${configuration.entityName} has been deleted successfully.`);
        },
        onAjaxError: (e: { xhr: XMLHttpRequest; error: string | Error }) => {
            debugger;
        },
        errorHandler: (e: Error) => {
            debugger;
        },
        onLoaded: (result) => {
            if (configuration.onLoaded) {
                configuration.onLoaded(result);
            }
            return result;
        },
    });
}

export function createGridDataSourceConfig<T>(
    configuration: { key: string; entityName: string },
    apiService: ApiService<T>,
    load: () => Observable<T[]>
): any {
    return {
        key: configuration.key,
        load: async (loadOptions): Promise<any> => {
            try {
                const data = await (load ? load() : apiService.getAll()).toPromise();
                return {
                    data,
                };
            } catch (error) {
                console.log(error);
                throw new Error('Data Loading Error');
            }
        },
        insert(values): Promise<any> {
            return apiService
                .post(values)
                .toPromise()
                .then((result) => {
                    showSuccessMessage(
                        `${configuration.entityName} has been created successfully.`
                    );
                    return result;
                })
                .catch((error) => {
                    console.log(error);
                    throw new Error('Error inserting data');
                });
        },
        remove(key): Promise<any> {
            return apiService
                .delete(key)
                .toPromise()
                .then(() => {
                    showSuccessMessage(
                        `${configuration.entityName} has been deleted successfully.`
                    );
                })
                .catch((error) => {
                    console.log(error);
                    throw new Error('Error deleting data');
                });
        },
        update(key, values): Promise<any> {
            return apiService
                .put(key, JSON.stringify(values))
                .toPromise()
                .then(() => {
                    showSuccessMessage(
                        `${configuration.entityName} has been updated successfully.`
                    );
                })
                .catch((error) => {
                    console.log(error);
                    throw new Error('Error updating data');
                });
        },
    };
}

// Creates a grid datasource that uses HttpClient for Api calls.
// By using HttpClient, we ensure that the Authorization token gets added to the requests via Interceptors.
export function createGridDataSource<T>(
    configuration: { key: string; entityName: string },
    apiService: ApiService<T>,
    load: () => Observable<T[]> = null
): DataSource {
    return new DataSource(createGridDataSourceConfig(configuration, apiService, load));
}
