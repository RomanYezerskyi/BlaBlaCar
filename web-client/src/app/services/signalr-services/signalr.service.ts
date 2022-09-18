import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, filter, Observable, Subject } from 'rxjs';
import { SignalEvent } from './signal-event';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private _signalEvent: Subject<SignalEvent<any>>;
  private _openConnection: boolean = false;
  private _isInitializing: boolean = false;
  private _hubConnection!: HubConnection;
  public url: string = '';
  public hubMethod: string = '';
  public hubMethodParams: any;
  public handlerMethod: string = '';

  constructor() {
    // super();
    console.log(this.url);
    this._signalEvent = new Subject<any>();
    // this._isInitializing = true;
    // this._initializeSignalR();
  }
  getDataStream<TData>(): Observable<SignalEvent<TData>> {
    console.log(this.hubMethod);
    this._ensureConnection<TData>();
    return this._signalEvent.asObservable();
  }
  private _ensureConnection<TData>() {
    if (this._openConnection || this._isInitializing) return;
    this._initializeSignalR<TData>();
  }
  private _initializeSignalR<TData>() {
    console.log(this.hubMethod);
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.url)
      .build();
    this._hubConnection.start()
      .then(_ => {
        this._openConnection = true;
        this._isInitializing = false;
        this._hubConnection.invoke(this.hubMethod,
          this.hubMethodParams)
        this._setupSignalREvents<TData>()
      })
      .catch(error => {
        console.warn(error);

        this._hubConnection.stop().then(_ => {
          this._openConnection = false;
          this._ensureConnection();
        })

      });

  }
  private _setupSignalREvents<TData>() {
    this._hubConnection.on(this.handlerMethod, (data) => {
      // map or transform your data as appropriate here:
      this._onMessage<TData>({ data })
    })
    // this._hubConnection.on('MessageNumberArray', (data) => {
    //   // map or transform your data as appropriate here:
    //   const { numbers } = data;
    //   this._onMessage({ type: SignalEventType.EVENT_TWO, data: numbers })
    // })
    this._hubConnection.onclose((e) => this._openConnection = false);
  }

  private _onMessage<TData>(payload: SignalEvent<TData>) {
    this._signalEvent.next(payload);
  }

}
