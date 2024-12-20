import { Component, OnInit } from '@angular/core';
import { TodoItem, TodoService } from './services/todo.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'ToDoAngularApp';

  todos: TodoItem[] = [];
  newTodoTitle: string = '';

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.todoService.getTodos().subscribe(todos => this.todos = todos);
  }

  addTodo(): void {
    if (!this.newTodoTitle.trim()) {
      return;
    }

    const newTodo: Partial<TodoItem> = { title: this.newTodoTitle };
    this.todoService.createTodo(newTodo).subscribe(todo => {
      this.todos.push(todo);
      this.newTodoTitle = '';
    });
  }

  completeTodo(todo: TodoItem): void {
    this.todoService.markAsCompleted(todo.id).subscribe(() => {
      todo.isCompleted = true;
    });
  }
}