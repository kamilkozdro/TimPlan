# TimPlan - Teams tasks planner and tracker
Application created to learn WPF and AvaloniaUI.

TimPlan is application for tasks planning and tracking. It allows to add multiple teams containing multiple users with different priviliges based on team role and system role. Tasks can be assigned only by user with specific privilege and are displayed as task tiles.

## Login window
User credential consists of: unique login name and password. In case of entering wrong credentials, appriopriate error is displayed.

![login_window](https://github.com/kamilkozdro/TimPlan/assets/8678160/51f3cdfa-04c8-46b1-9711-23915187df8e)

## Application window
Application window consists of upper toolbar and main section.
Toolbar shows currently logged username, contains buttons for login/logout and if logged user has admin System role, additional buttons for editing (users, teams, team roles, etc) are displayed.
Main section contains 2 tabs:
- My Tasks - displays task assigned to logged user
  ![MyTask_admin](https://github.com/kamilkozdro/TimPlan/assets/8678160/fd033c47-1f3c-44f2-814d-1ca4c915743c)
- Team Overview - displays tasks assigned to team member currenty selected from list. Also by pressing green "+" button, a task can be created and assigned to selected user.
  ![Przemek_tasks](https://github.com/kamilkozdro/TimPlan/assets/8678160/f9ad2507-675e-4156-ad9f-3ddfa774c1b1)



## Task

Task's properties:
- Name - short task name, displayed in task tile
- Start Date - when the task should Begin
- End Date - deadline of the task
- Created Date  - date when the task was created (uneditable)
- Team - team assigned for the task
- User - user from selected team assigned for the task
- Parent Task - the task can be assigned to another main (parent) task
- Description - detailed description of the task

Created tasks are shown as task tiles, which are shortened representation of the task displaying most important properties:
- Name of the task
- Days remaining before the deadline (end date)
- Color of tile representing task state ( yellow - suspended, blue - accepted, green - completed)
- Description of the task shown in tooltip by hovering cursor over tile

User assigned to the task can change it's state by clicking appriopriate buttons on tile (Accept, Completed, Suspended).
Task tile also has small information button "i" in upper right corner, which opens up a window with complete informations about task. Users with specific priviliges can also edit the task this way.

![task_editmode](https://github.com/kamilkozdro/TimPlan/assets/8678160/b43a8391-b7db-483f-85d2-8773212373af)



