import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, catchError } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { InjectionToken } from '@angular/core';
import { Inject } from '@angular/core';

// Define an injection token for the URL
export const API_URL = new InjectionToken<string>('API_URL');
@Injectable()
export class DataService {
    constructor(@Inject(API_URL) private url: string, private http: HttpClient) { 
        if (!url) throw Error("No Url provided or injected.")
            this.url = url;
    }

    getById(id: number) {
        return this.http.get(`${this.url}/${id}`)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    }

    getAll() {
        return this.http.get(this.url)
        .pipe(map(response => response)
            , shareReplay() 
            , catchError(this.handleError)
             );
    }

    create(resource: any) {
        return this.http.post(this.url, resource)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    
    }
    
    update(resource: any) {
        return this.http.put(this.url, resource)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    }

    delete(id: number) {
        return this.http.delete(`${this.url}/${id}`)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    }

    //Add these methods to allow the service to provide the url in the call
    //in case the url has a different endpoint such as User/Login, User/CreateUser
    urlGetById(urlExt: string, id: number) {
        return this.http.get(`${this.url}/${urlExt}/${id}`)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    }
    
    urlGetAll(urlExt: string) {
        return this.http.get(`${this.url}/${urlExt}`)
        .pipe(map(response => response)
            , shareReplay() 
            , catchError(this.handleError)
             );
    }

    urlCreate(urlExt:string, resource: any) {
        return this.http.post(`${this.url}/${urlExt}`, resource)
        .pipe(map(response => response)
            , shareReplay(1) 
            , catchError(this.handleError)
             );
    
    }

    private handleError(error: any): Observable<never> {
        console.error('An error occurred', error);
        return throwError(error.message || error);
    }
}