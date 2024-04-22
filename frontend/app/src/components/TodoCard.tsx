import { Card, CardContent, CardDescription, CardFooter, CardTitle } from '@/components/ui/card'
import { cn } from '@/lib/utils'
import { Todo } from '@/schemas'
import { CheckCircledIcon, CircleIcon, ClockIcon } from '@radix-ui/react-icons'
import dayjs from 'dayjs'
import TodoCardActions from './TodoCardActions'
import { Badge } from './ui/badge'

const statuses = [
  {
    value: false,
    label: 'Todo',
    icon: CircleIcon,
    class: 'text-blue-600 border-blue-600'
  },
  {
    value: true,
    label: 'Done',
    icon: CheckCircledIcon,
    class: 'text-green-600 border-green-600'
  }
]

const getStatus = (completed: boolean) => {
  return statuses.find((s) => s.value === completed)
}

type TodoCardProps = {
  data: Todo
}

export function TodoCard({ data }: TodoCardProps) {
  const status = getStatus(data.is_complete)

  return (
    <>
      <Card className='w-full'>
        <div className='flex items-center justify-between pl-6 pr-4 pt-2 mb-4'>
          <CardTitle className={'text-slate-600 text-base'}>{`#${data.id}`}</CardTitle>
          <Badge variant='outline' className={cn(`border font-normal h-5`, status?.class)}>
            {status?.label}
          </Badge>
          <TodoCardActions data={data} />
        </div>
        <CardContent className='min-h-20'>
          <CardDescription className='line-clamp-3'>{data.description}</CardDescription>
        </CardContent>
        <CardFooter className='flex items-center justify-between space-x-4'>
          <div className='flex items-center justify-start space-x-2'>
            <ClockIcon /> <span className='text-foreground'>{dayjs(data.updated_at).format('YYYY-MM-DD HH:mm')}</span>
          </div>
        </CardFooter>
      </Card>
    </>
  )
}

export default TodoCard
