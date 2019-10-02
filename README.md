# Common Classes used in .NET Projects

# Result class
Always prefer using return values over exception for control flow.
Use cases for Exceptions:
  - Exceptional situations (3rd party libraries). Own code is non-exceptional situation
  - Exceptions should signalize a bug
  - Unexpected failures
  - Don't use exceptions in situations you expect to happen 

Catch exceptions at highest possible level for logging purposes and at lowest possible level from 3rd party libraries. 

Use return values to define an excepted failure
Result class:
  - Keep the result of operation together with its status
  - Unified succes/error model with indicator of succes and message
  - Use only for expected failures

### The Result class and CQS 
| Method signature |  |
| ------ | ------ |
| public void Save (T value) | Command / Not expected to fail |
| public Result Save (T value) | Command / Expected to fail |
| public T Get(int id) | Query / Not excpected to fail |
| public Result<T> Get(int id) | Query / Expected to fail |

# Maybe class
- Make your code honest with Maybe type.
- Convert nulls into Maybe when they enter the domain model.
- Convert them back to nulls when they leave the domain model.
- Result<T> != Maybe<T>
- Result<T> GetById(int id) :(
- Maybe<T> GetById(int id) :)
