import { useState } from 'react'
import { Button } from '@/components/ui/button'
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu'
import { useUpdateTodo } from '@/hooks/useTodo'
import { Todo, UpdateTodo } from '@/schemas'
import { CheckCircledIcon, CircleIcon, DotsHorizontalIcon, Pencil1Icon, TrashIcon } from '@radix-ui/react-icons'
import { toast } from 'sonner'
import { TodoAddDialog } from './TodoAddDialog'
import { TodoDeleteDialog } from './TodoDeleteDialog'

type TodoCardActionsProps = {
  data: Todo
}

export function TodoCardActions({ data }: TodoCardActionsProps) {
  const [showDeleteDialog, setShowDeleteDialog] = useState(false)
  const [showEditDialog, setShowEditDialog] = useState(false)

  const nextStatus = data.is_complete ? 'todo' : 'done'
  const msg = data.is_complete ? 'Marked as todo' : 'Marked as done'
  const { trigger } = useUpdateTodo()

  const changeTodoStatus = () => {
    const payload: UpdateTodo = { ...data, is_complete: !data.is_complete, updated_at: new Date().toISOString() }
    trigger(payload, {
      onSuccess: () => {
        setShowEditDialog(false)
        toast.success(msg)
      },
      onError: (err) => {
        console.error(err)
        toast.warning('Failed to update todo')
      }
    })
  }
  return (
    <>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant='ghost' className='flex h-8 w-8 p-0 data-[state=open]:bg-muted'>
            <DotsHorizontalIcon className='h-4 w-4' />
            <span className='sr-only'>Open menu</span>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align='start' className='w-[160px]' side='right'>
          <DropdownMenuItem className='flex space-x-2 items-center justify-start' onSelect={() => setShowEditDialog(true)}>
            <Pencil1Icon className='text-gray-500' />
            <span>Edit</span>
          </DropdownMenuItem>
          <DropdownMenuItem className='flex space-x-2 items-center justify-start' onSelect={changeTodoStatus}>
            {data.is_complete ? <CircleIcon className='text-blue-600' /> : <CheckCircledIcon className='text-green-600' />}
            <span>Mark as {nextStatus}</span>
          </DropdownMenuItem>
          <DropdownMenuItem className='flex space-x-2 items-center justify-start' onSelect={() => setShowDeleteDialog(true)}>
            <TrashIcon className='text-red-500 w-4 h-4' />
            <span className='text-red-500'>Delete</span>
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
      <TodoAddDialog initialData={data} isEdit={true} setShowAddTodo={setShowEditDialog} showAddTodo={showEditDialog} />
      <TodoDeleteDialog data={data} open={showDeleteDialog} onOpenChange={setShowDeleteDialog} />
    </>
  )
}

export default TodoCardActions
