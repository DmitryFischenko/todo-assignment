# Todo List Assignment

## Server side

### Projects overview

#### TodoManager
Business model and public interfaces.

#### TodoManager.Implementation
Business logic implemntation

#### TodoManager.DataAccess
Storage agnostic DTO model and repository interface

#### TodoManager.DataAccess.SQLite
SQLite specific repository implementation

#### TodoManager.Web
Host and API implementation

### Database and ORM
- SQLite + Entity Framework 

### Exception Handling in Web project
Exception to HttpCode conversion implemented in pipeline, in `GlobalExceptionHandler.cs`.
- `BadHttpRequestException` converted to HttpStatus passed as a parameter (400, 404)
- Subclasses of `BusinessException` converted to 422 HttpStatus

### Logging
For Logging Serilog for .NET is used. Configuration is in `appsettings.json`

### Dilemmas
- More contracts but relevant fields only vs shared model with immutable fields
Here I decided to go with shared model as the number of irrelevant fields is low (it's actually one). But if had, for instance, such by-server-populated fields as DateCreated, DateUpdated, Timestamp etc, I would have different contracts for input and output data on API and BL layers. 


### What I left behind due to lack of time
- UTs are added not for all methods
 - No dedicated tests for Mapper (no custom field mapping in this solution though). Conversion logic partially covered in other tests.
- Logging added for few operations only (delete item, insert item)
- Comments on public interfaces/members.
- For Serilog, haven't updated the format to include class and member names to a log record


### What I might consider to change/add
- Replace integer ID with GUID for data migration, replication etc purposes
- Swagger for 
 - API documentation
 - posible UI API client code generation
- Even though, it's not trully RESTful and there is no side effects of marking item 'completed' in the current implementation, for this operation I might prefer a route similar to:
```
/todo-items/{id}/actions/{action}
```
where action is enumration: { markAsCompleted, markAsActive}
 

