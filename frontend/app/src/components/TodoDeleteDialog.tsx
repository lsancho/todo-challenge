import { useDeleteTodo } from '@/hooks/useTodo'
import { Todo } from '@/schemas'
import { toast } from 'sonner'
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle
} from './ui/alert-dialog'
import { Button } from './ui/button'

type TodoDeleteDialogProps = {
  data: Todo
  open: boolean
  onOpenChange: React.Dispatch<React.SetStateAction<boolean>>
}

export function TodoDeleteDialog({ data, open, onOpenChange }: TodoDeleteDialogProps) {
  const { trigger } = useDeleteTodo()

  const handleDeleteTodo = () => {
    onOpenChange(false)
    trigger(data, {
      onSuccess: () => {
        toast.info('Todo has been deleted')
      }
    })
  }

  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you sure?</AlertDialogTitle>
          <AlertDialogDescription>This is irreversible. Please proceed with caution.</AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel>Cancel</AlertDialogCancel>
          <Button variant='destructive' onClick={handleDeleteTodo}>
            Delete
          </Button>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}

export default TodoDeleteDialog
