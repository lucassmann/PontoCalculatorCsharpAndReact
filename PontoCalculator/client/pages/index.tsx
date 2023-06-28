import styles from '../styles/Home.module.css'
import Link from 'next/link'

export default function Home() {
  return (
    <section className={styles.container}>
      <h1>Bem -vindo ao <span>Ponto Calculator</span></h1>
      <p>Comece a gerenciar os seus turnos de trabalho agora mesmo!</p>
      <Link href="/login">Entre em sua conta!</Link>
      <br/>
      <Link href="/cadastro">Ainda não possui conta?</Link>
    </section>
    
  )
}
