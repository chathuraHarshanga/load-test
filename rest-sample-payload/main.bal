import ballerina/http;

configurable string name = ?;

service / on new http:Listener(8090) {
    resource function get greet() returns string {
        return "Hello, " + name + "!";
    }
}
