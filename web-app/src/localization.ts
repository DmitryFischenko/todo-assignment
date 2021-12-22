import LocalizedStrings from 'react-localization';

let strings = new LocalizedStrings({
 en:{
   newItemPrompt:"Type and hit Enter to add a new item",
   lbl_activeTasks:"Active Tasks",
   lbl_completedTasks:"Completed Tasks",
   title:"Todo Manager",
   deleteConfirmation: 'Are you sure delete this task?',
   deleteConfirmationDesc: 'The task will be deleted permanently',
   yes: 'yes',
   no: 'no',
   input_error_empty: 'Please provide Todo item text',
   error_status_422_1: 'Item with such already exists'
 }
});

export default strings;