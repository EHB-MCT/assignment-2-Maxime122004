# C# Coding Conventions
## Naming Conventions
1. **Variables**: 
   - Use `camelCase` for all variable names.
    - Example: `playerHealth`, `jumpForce`.
2. **Constants**: 
   - Use `UPPERCASE_SNAKE_CASE` for constant values. 
    - Example: `MAX_JUMP_HEIGHT`, `DEFAULT_SPEED`.
3. **Functions**: 
   - Use `PascalCase` for function and method names.
    - Example: `MovePlayer()`, `CalculateScore()`.
4. **Classes**: 
   - Use `PascalCase` for class names.
    - Example: `PlayerController`, `GameManager`.
5. **Namespaves**:
   - Use `PascalCase` for namespace names, matching the folder structure.
    - Example: `Game.Utils`, `Game.UI`.
## Indentation
- Use **4 spaces** for indentation.
- Avoid using tabs to ensure consistent formating across different environments.
## Line Length
- Limit lines to **100 characters** for better readability.
- If a line exceeds 100 characters, break it into multiple lines following logical groupings.
## Comments
1. **Single-Line Comments**:
   - Use `//` for in-line explanations or to describe specific lines of code.
2. **Javadoc-stule Comments**:
   - Use `/**...*/` for functions documentation before a function.
## File Structure
1. **Class file**:
   - Place each class in its own file. The filename must match the class name
    - Example: `PlayerController.cs` -> Contains the PlayerController class.
## Additional Conventions
1. **Method Length**:
   - Keep methods concise and focused on a single responsibility.
   - If a method becomes too long, refractor it into smaller, reusable methods.
2. **Error Handling**:
   - Use `try-catch` blocks for error-prone code and log exceptions usinga centralized logging mechanism.
3. **Debugging**:
   - Use `Debug.Log()` for temporary debugging but remove these statements in the final production build.