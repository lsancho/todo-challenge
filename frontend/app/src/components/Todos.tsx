import { useState } from 'react'
import { useTodos } from '@/hooks/useTodo'
import { Todo } from '@/schemas'
import { PlusIcon } from '@radix-ui/react-icons'
import EmptyTodo from './EmptyTodo'
import TodoAddDialog from './TodoAddDialog'
import TodoCard from './TodoCard'
import { Button } from './ui/button'
import { Spinner } from './ui/spinner'

export default function Todos() {
  const { todos, isLoading } = useTodos()

  console.debug('todos', { todos, isLoading })

  if (isLoading)
    return (
      <div className='flex items-center justify-center h-[80vh]'>
        <Spinner />
      </div>
    )

  if (!todos?.length) {
    return <EmptyTodo />
  }

  return <TodoCardView data={todos} />
}

function TodoCardView({ data }: { data: Todo[] }) {
  const [showAddTodo, setShowAddTodo] = useState(false)

  return (
    <div className='space-y-4'>
      <div className='flex items-center justify-between'>
        <div className='flex flex-col-reverse md:flex-row flex-1 items-center space-x-2 gap-4'>
          <div className='flex space-x-4'>
            <Button size='sm' className='ml-auto h-[30px] flex' onClick={() => setShowAddTodo(true)}>
              <PlusIcon className='mr-2 font-bold h-4 w-4' /> Todo
            </Button>
          </div>
        </div>
      </div>
      <TodoAddDialog setShowAddTodo={setShowAddTodo} showAddTodo={showAddTodo} />
      <div className='grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 rounded-md'>
        {data.map((todo, i) => (
          <TodoCard key={i} data={todo} />
        ))}
      </div>
    </div>
  )
}
