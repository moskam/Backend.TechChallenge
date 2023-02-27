# Author comments:
- .NET Core 3.1 is not longer supported, I have upgraded to .NET6 the solution.
- I have assumed that this solution is a refactoring of already working solution.
  Therefore it had to be backward compatible.
- Backward compatibility is broken in case of invalid request.
  Now it will be returing 400 Bad Request with validation errors details,
  before it was returning 200, with concatenate error messages.
- I have used TDD for refactoring. First I have focused on tests, to make sure,
  do not break current functionality, while I was doing refactoring.
- I have tried to cover most of the cases keeping existing functionality intack at the same time.
  There might be some bugs, which were in the code before, like for example when user is normal
  and money is 100, it will not be calculated or normalize email functionality.
- I did not see good place to use mocking framework. I current project we are using using
  NSubstitute, but I am familiar with MOQ and RhinoMocks as well.
- To read and write data I have used repository pattern. I assume no one is manipulating
  data in file directly, so data integrity should be ensured on the application level.
