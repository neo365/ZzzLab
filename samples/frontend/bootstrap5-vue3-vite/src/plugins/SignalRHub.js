import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";

const builder = new HubConnectionBuilder();
const connection = builder
    .withUrl('http://localhost:8089/NotifyHub')
    .configureLogging(LogLevel.Debug)
    .withAutomaticReconnect({nextRetryDelayInMilliseconds: () => 60000}) //
    .build();

connection.on("DebugMessage", (message) => {
  console.log("DebugMessage", message);
  this.emitter.emit('DebugMessage', message)
});

connection.onreconnected((connectionId) => {
  console.log(connectionId, "onreconnected");
});


connection.onclose((res) => {
  console.log("*", res);
});

if (connection.state !== HubConnectionState.Connected) {
  connection.start().then((res) => {
    console.log("", res);
  });
}