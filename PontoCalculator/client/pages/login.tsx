import Link from "next/link";
import styles from "../styles/login.module.css";
import LoginCard from "../src/components/loginCard/loginCard";
import React, { SyntheticEvent, useState } from "react";
import { useRouter } from "next/router";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();

  const handleSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    await fetch("http://localhost:5127/api/Auth/login", {
      method: "POST",
      headers: {'Content-Type': 'application/json'},
      credentials: 'include',
      body: JSON.stringify({
        email,
        password,
      }
      ),});
     await router.push("/");
  };

    return (
        <div className={styles.background}>
            <LoginCard title="Entre em sua conta">
              <form className={styles.form} onSubmit={handleSubmit}>  
                <input className={styles.input} type="email" placeholder="Seu e-mail" required onChange={e => setEmail(e.target.value)}/>
                <input className={styles.input} type="password" placeholder="Sua senha" required onChange={e => setPassword(e.target.value)}/>
                <button type="submit" className={styles.button}>Entrar</button>
                <Link href="/cadastro">Ainda não possui conta?</Link>
                <Link href="/recuperar-senha">Lembrar login e senha?</Link>
              </form>
            </LoginCard>
        </div>
    );
};
export default Login;