import { AddTodo, DeleteTodo, Todo, UpdateTodo } from '@/schemas'
import { addTodo, deleteTodo, getAll, updateTodo } from '@/services'
import useSWR from 'swr'
import useSWRMutation from 'swr/mutation'

type AddResult = Todo | Error
type UpdateResult = Todo | Error
type DeleteResult = DeleteTodo | Error
type CacheType = Todo[] | undefined

const cacheSWRKey = 'todos'

export const useTodos = () => {
  const { data, error, isLoading } = useSWR<Todo[], Error>(cacheSWRKey, getAll)
  return {
    todos: data,
    isLoading,
    isError: error
  }
}

export const useAddTodo = () => {
  const { trigger, isMutating, error } = useSWRMutation<AddResult, Error, string, AddTodo, CacheType>(cacheSWRKey, (_, { arg }) => addTodo(arg), {
    revalidate: false,
    populateCache: (result, currentData) => {
      if (result instanceof Error) {
        return currentData
      }
      console.debug('add:populateCache', { result, currentData })
      const rest = currentData ? currentData.filter((todo) => todo.id !== result.id) : []
      return currentData ? [...rest, result] : [result]
    }
  })

  return {
    trigger,
    isMutating,
    isError: error
  }
}

export const useUpdateTodo = () => {
  const { trigger, isMutating, error } = useSWRMutation<UpdateResult, Error, string, UpdateTodo, CacheType>(
    cacheSWRKey,
    (_, { arg }) => updateTodo(arg),
    {
      revalidate: false,
      populateCache: (result, currentData) => {
        if (result instanceof Error) {
          return currentData
        }
        const rest = currentData ? currentData.filter((todo) => todo.id !== result.id) : []
        return currentData ? [...rest, result] : [result]
      }
    }
  )

  return {
    trigger,
    isMutating,
    isError: error
  }
}

export const useDeleteTodo = () => {
  const { trigger, isMutating, error } = useSWRMutation<DeleteResult, Error, string, DeleteTodo, CacheType>(
    cacheSWRKey,
    (_, { arg }) => deleteTodo(arg),
    {
      revalidate: false,
      populateCache: (result, currentData) => {
        if (result instanceof Error) {
          return currentData
        }
        return currentData ? currentData.filter((todo) => todo.id !== result.id) : []
      }
    }
  )

  return {
    trigger,
    isMutating,
    isError: error
  }
}
