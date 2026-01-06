import ballerina/http;


service / on new http:Listener(8090) {
    resource function get greet(string name) returns string {
        return "Hello, " + name + "!";
    }
}