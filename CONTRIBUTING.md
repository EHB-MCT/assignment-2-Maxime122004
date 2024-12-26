# Contributing to This Project
## Contribution Guidelines
1. **Folder Structure and Naming Conventions**:
   - Follow the existing folder structure stated in the README and use semantic names for any files or folders.
   - Ensure assets and scripts are placed in their appropriate directories (e.g., scripts in `Assets/Scripts`, UI assets in `Assets/UI`).
2. **Code Documentation**:
   - Use Javadoc-style comments (`/**...*/`) for all new scripts and function.
     - Include information about inputs, actions and outputs.
   - If modifying existing code, update comments where applicable to reflect changes.
3. **Testing Requirements**:
   - Thoroughly test all changes before submitting pull requests.
   - Verify that the project builds and runs without errors or warnings.
   - Ensure all new features function as intended, and check compatability with existing features.
4. **Dependencies**:
   - Add new dependencies only if necessary. Ensure they are properly documented and included in the project setup intructions.
## Branching Strategy
1. **Main Branch**:
   - Contains stable production-ready code.
2. **Feature Branches**:
   - Develop new features in branches named `feature/[name]` (e.g., `feature/new-enemy`).
## Commit Guidelines
- Use clear and descriptive commit messages.
- Use the following commit prefixes:
   - `feat: [description]` for new features.
   - `fix: [description]` for bug fixes.
   - `chore: [description]` for maintenance tasks.
   - `refractor: [description]` for clean up or improvements.
   - `docs: [description]` for markdown files.
## Pull Requests
1. **Coding Conventions**:
   - Ensure your code follows the conventions outlined in `CONVENTIONS.md`.
   - Maintain readability and adhere to modular design principles.
2. Pull Request Description:
   - Provide a clear and detailed explanation of the changes you made.
   - Mention the purpose of the change, relevant issue numbers (if any) and testing steps.
3. **Review Process**:
   - All pull requests will be reviewed for quality consistency, and adherence to the guidelines before being merged.

## Additional Notes
- If you have questions about contributing or are unsure about a change, feel free to open a discussion or issue in the repository.
- Contributions of all levels are welcome, from small bug fixes to major feature implementations.

By adhering to these guidelines, you help ensure the project remains organized, efficient, and high-quality.