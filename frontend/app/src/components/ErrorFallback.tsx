export default function ErrorFallback() {
  return (
    <div className='flex items-center justify-center h-[80vh]'>
      <div className='text-center'>
        <h2 className='text-2xl font-semibold'>Something went wrong</h2>
        <p className='text-gray-500'>Please refresh the page or try again later.</p>
      </div>
    </div>
  )
}
