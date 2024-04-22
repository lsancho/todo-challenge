import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog'
import { useAddTodo, useUpdateTodo } from '@/hooks/useTodo'
import { AddTodo, addTodoSchema, Todo, UpdateTodo } from '@/schemas'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { Button } from './ui/button'
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from './ui/form'
import { Textarea } from './ui/textarea'

type TodoAddDialogProps = {
  showAddTodo: boolean
  setShowAddTodo: React.Dispatch<React.SetStateAction<boolean>>
  isEdit?: boolean
  initialData?: Todo
}

type AddTodoFormValue = AddTodo

export function TodoAddDialog({ showAddTodo, setShowAddTodo, isEdit = false, initialData }: TodoAddDialogProps) {
  const { trigger: addTodo } = useAddTodo()
  const { trigger: updateTodo } = useUpdateTodo()

  const form = useForm<AddTodoFormValue>({
    defaultValues: initialData,
    resolver: zodResolver(addTodoSchema),
    mode: 'onChange'
  })

  function onSubmit(data: AddTodoFormValue) {
    const onSuccess = () => {
      setShowAddTodo(false)
      toast.success(isEdit ? 'Todo has been updated' : 'Todo has been added')
    }

    const payload = { ...data, updated_at: new Date().toISOString() }

    if (isEdit) updateTodo({ ...initialData, ...payload } as UpdateTodo, { onSuccess })
    else addTodo({ ...payload } as AddTodo, { onSuccess })

    form.reset()
  }

  return (
    <Dialog open={showAddTodo} onOpenChange={setShowAddTodo}>
      <DialogContent className='w-[350px] lg:w-[450px]'>
        <DialogHeader>
          <DialogTitle>Create Todo</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-8'>
            <FormField
              control={form.control}
              name='description'
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description</FormLabel>
                  <FormControl>
                    <Textarea {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button type='submit'>{isEdit ? 'Update' : 'Add'}</Button>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}

export default TodoAddDialog
