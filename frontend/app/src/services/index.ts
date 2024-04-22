import { AddTodo, DeleteTodo, GetTodo, Todo, UpdateTodo } from '@/schemas'

const getApiBaseUrl = () => {
  const { VITE_API_HOST, VITE_API_PORT } = import.meta.env
  return `http://${VITE_API_HOST}:${VITE_API_PORT}`
}

export async function getAll(): Promise<Todo[]> {
  const url = getApiBaseUrl() + '/todos'
  console.debug('getAll', url)
  return await fetch(url).then((res) => res.json())
}

export async function get(arg: GetTodo): Promise<Todo> {
  const url = getApiBaseUrl() + '/todos/' + arg.id
  return await fetch(url).then((res) => res.json())
}

export async function addTodo(arg: AddTodo): Promise<Todo | Error> {
  const url = getApiBaseUrl() + '/todos'
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(arg)
  })

  if (response.ok) {
    return await response.json()
  } else {
    const err = await response.json()
    throw err
  }
}

export async function updateTodo(arg: UpdateTodo): Promise<Todo | Error> {
  const url = getApiBaseUrl() + '/todos/' + arg.id
  console.debug('updateTodo', url, arg)
  const response = await fetch(url, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(arg)
  })

  if (response.ok) {
    return await response.json()
  } else {
    console.debug('updateTodo:error')
    const err = await response.json()
    throw err
  }
}

export async function deleteTodo(arg: DeleteTodo): Promise<DeleteTodo | Error> {
  const url = getApiBaseUrl() + '/todos/' + arg.id
  const response = await fetch(url, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(arg)
  })

  if (response.ok) {
    return arg
  } else {
    const err = await response.json()
    throw err
  }
}
