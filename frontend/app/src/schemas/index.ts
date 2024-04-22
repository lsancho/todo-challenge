import { z } from 'zod'

export const todoSchema = z.object({
  id: z.string(),
  description: z.string(),
  is_complete: z.boolean(),
  updated_at: z.string().datetime()
})

export type Todo = z.infer<typeof todoSchema>

export const getTodoSchema = z.object({
  id: z.string()
})

export type GetTodo = z.infer<typeof getTodoSchema>

export const addTodoSchema = z.object({
  description: z
    .string()
    .min(10, {
      message: 'Description must be at least 10 characters long.'
    })
    .max(2000, {
      message: 'Description must be less than 2000 characters long.'
    })
})

export type AddTodo = z.infer<typeof addTodoSchema>

export const updateTodoSchema = z.object({
  id: z.string(),
  description: z
    .string()
    .min(10, {
      message: 'Description must be at least 10 characters long.'
    })
    .max(2000, {
      message: 'Description must be less than 2000 characters long.'
    }),
  is_complete: z.boolean(),
  updated_at: z.string().datetime()
})

export type UpdateTodo = z.infer<typeof updateTodoSchema>

export const deleteTodoSchema = z.object({
  id: z.string()
})

export type DeleteTodo = z.infer<typeof deleteTodoSchema>

const statusSchema = z.enum(['todo', 'completed'])
export type Status = z.infer<typeof statusSchema>
