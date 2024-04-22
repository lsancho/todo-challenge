import Todos from './components/Todos'

function App() {
  console.debug('env', import.meta.env)

  return (
    <main className='w-full max-w-7xl mx-auto pb-8 h-[calc(100vh-90px)] flex flex-col space-y-10 px-8 mt-8'>
      <div>
        <Todos />
      </div>
    </main>
  )
}

export default App
