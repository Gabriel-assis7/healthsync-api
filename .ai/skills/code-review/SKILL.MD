# C# / .NET Code Review Guide

> A comprehensive C# / .NET 8 code review guide covering C# 12 language features, asynchronous programming, EF Core performance, ASP.NET Core best practices, dependency injection, LINQ, and modern coding guidelines.

## Table of Contents

* C# 12 Features
* Asynchronous Programming
* EF Core Performance
* ASP.NET Core Best Practices
* Dependency Injection
* LINQ Best Practices
* Review Checklist

---

# C# 12 Features

## Primary Constructors

### Prefer Primary Constructors for Dependency Injection

* Use primary constructors where appropriate to reduce boilerplate.
* Remember that primary constructor parameters are **not properties**.
* If a dependency needs to be stored for long-term use, explicitly capture it in a readonly field.

Review questions:

* Is a traditional constructor necessary?
* Are constructor parameters unnecessarily copied into fields?
* Are dependencies clearly expressed?

---

## Collection Expressions

Prefer collection expressions whenever possible.

Instead of older initialization syntax, prefer the C# 12 collection expression syntax for arrays, lists, spans, and merged collections.

Review questions:

* Can this collection use collection expressions?
* Is the syntax consistent throughout the project?

---

## Default Lambda Parameters

Prefer default lambda parameters over unnecessary overloads when appropriate.

Review questions:

* Are multiple overloads only providing default values?
* Would a default parameter improve readability?

---

# Asynchronous Programming

## Avoid Blocking Async Code

Never use:

* `Task.Wait()`
* `.Result`
* `.GetAwaiter().GetResult()`
* `async void` (except event handlers)

Prefer fully asynchronous code paths.

Review questions:

* Is the async flow preserved end-to-end?
* Are any blocking calls present?

---

## ConfigureAwait(false)

For reusable libraries (not ASP.NET Core applications), use:

* `ConfigureAwait(false)`

Avoid unnecessary synchronization context capture.

---

## CancellationToken Propagation

Always propagate `CancellationToken` through every async operation.

Review questions:

* Is the token accepted?
* Is it forwarded to database calls?
* Is it forwarded to HTTP calls?
* Is it forwarded to external services?

---

## Async Disposal

Prefer:

* `IAsyncDisposable`
* `await using`

for asynchronous resources.

---

# EF Core Performance

## Avoid N+1 Queries

Never access navigation properties inside loops unless the data has already been loaded.

Prefer:

* projection
* eager loading
* split queries when appropriate

Review questions:

* Is every navigation property already loaded?
* Could this produce additional SQL queries?

---

## Project Only Required Columns

Avoid loading entire entities when only a subset of fields is required.

Prefer projections using `Select()`.

---

## Always Paginate Large Queries

Never return unbounded result sets.

Prefer:

* `Skip()`
* `Take()`
* deterministic ordering

---

## Use AsSplitQuery() When Appropriate

When multiple `Include()` statements are used, consider:

* `AsSplitQuery()`

to avoid cartesian explosion.

---

## Use AsNoTracking() for Read-Only Queries

Read-only queries should disable change tracking.

Review questions:

* Is the entity being modified?
* If not, can `AsNoTracking()` be used?

---

## Keep Queries Index-Friendly

Avoid expressions that prevent index usage.

Examples include:

* `ToLower()`
* `EndsWith()`
* functions applied directly to indexed columns

Prefer sargable predicates whenever possible.

---

## Prefer Async Database Operations

Always use:

* `ToListAsync()`
* `FirstOrDefaultAsync()`
* `SaveChangesAsync()`

instead of synchronous alternatives.

---

# ASP.NET Core Best Practices

## Use IHttpClientFactory

Never instantiate `HttpClient` directly for every request.

Prefer dependency injection with `IHttpClientFactory`.

---

## Don't Capture Scoped Services in Background Tasks

Background operations must create their own dependency injection scope.

Never reuse request-scoped services after the request has completed.

---

## Read Forms Asynchronously

Prefer:

* `ReadFormAsync()`

instead of synchronous access through `Request.Form`.

---

## Don't Use Exceptions for Control Flow

Prefer methods such as:

* `FirstOrDefaultAsync()`
* `SingleOrDefaultAsync()`
* `AnyAsync()`

instead of relying on exceptions.

---

## Modify Response Headers Safely

If headers must be modified after middleware execution, use:

* `Response.OnStarting()`

instead of modifying headers after the response body has started.

---

# Dependency Injection

## Respect Service Lifetimes

Avoid injecting:

* Scoped services into Singleton services.

Create a new scope when background services need scoped dependencies.

Review questions:

* Are service lifetimes compatible?
* Is scope creation necessary?

---

# LINQ Best Practices

## Avoid Premature Materialization

Don't call `ToList()` before all database-side filtering has been completed.

Allow the database to execute as much of the query as possible.

---

## Prefer Any() Over Count() > 0

Use:

* `Any()`
* `AnyAsync()`

when checking for existence.

---

## Avoid Multiple Enumeration

If an `IEnumerable<T>` will be enumerated multiple times, materialize it once.

Review questions:

* Is the sequence enumerated repeatedly?
* Could this trigger multiple database queries?

---

## Avoid Side Effects Inside Select()

`Select()` should remain a projection.

Move logging, mutations, and other side effects to a separate `foreach` or dedicated processing step.

---

# General Review Guidelines

During code reviews, prioritize:

* Readability
* Maintainability
* Performance
* Correctness
* Simplicity
* Consistency
* SOLID principles
* Clean Architecture
* Proper async usage
* Proper dependency injection
* Efficient database access
* Clear separation of concerns

Avoid introducing unnecessary complexity.

Prefer explicit, maintainable, and self-documenting code.

---

# Review Checklist

## C# 12

* [ ] Primary constructors are used appropriately.
* [ ] Collection expressions are preferred where applicable.
* [ ] Default lambda parameters simplify the implementation.

## Async Programming

* [ ] No `Task.Wait()`, `.Result`, or `async void`.
* [ ] `ConfigureAwait(false)` is used in reusable libraries.
* [ ] `CancellationToken` is propagated.
* [ ] Async resources implement `IAsyncDisposable`.
* [ ] Database operations are asynchronous.

## EF Core

* [ ] No N+1 queries.
* [ ] Proper projections are used.
* [ ] Pagination exists for large queries.
* [ ] `AsSplitQuery()` is used when beneficial.
* [ ] Read-only queries use `AsNoTracking()`.
* [ ] Queries remain index-friendly.
* [ ] Database access is asynchronous.

## ASP.NET Core

* [ ] `IHttpClientFactory` is used.
* [ ] Scoped services are not captured by background tasks.
* [ ] Forms are read asynchronously.
* [ ] Exceptions are not used for normal control flow.
* [ ] Response headers are modified safely.

## Dependency Injection

* [ ] Service lifetimes are respected.
* [ ] Background services create their own scopes.

## LINQ

* [ ] Queries avoid unnecessary materialization.
* [ ] `Any()` is preferred over `Count() > 0`.
* [ ] Enumerables are not enumerated multiple times.
* [ ] `Select()` contains no side effects.
