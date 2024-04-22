import { useState } from 'react'
import { PlusIcon } from '@radix-ui/react-icons'
import { TodoAddDialog } from './TodoAddDialog'
import { Button } from './ui/button'

function EmptyTodo() {
  const [showAddTodo, setShowAddTodo] = useState(false)

  return (
    <>
      <div className='w-full flex items-center justify-center h-[70vh]'>
        <div className='w-60 lg:w-80 relative h-64 flex items-end'>
          <div className='flex flex-col items-center justify-center w-full'>
            <h3 className='text-sm'>No todos found</h3>
            <p className='text-gray-500 text-xs'>Add a new todo to get started</p>
            <Button size='lg' className=' flex mt-4' onClick={() => setShowAddTodo(true)}>
              <PlusIcon className='mr-2 font-bold h-4 w-4' /> Todo
            </Button>
          </div>
        </div>
      </div>
      <TodoAddDialog setShowAddTodo={setShowAddTodo} showAddTodo={showAddTodo} />
    </>
  )
}

export default EmptyTodo
