import Link from "next/link";
import styles from "../styles/login.module.css";
import LoginCard from "../src/components/loginCard/loginCard";
import React, { SyntheticEvent, useState } from "react";
import { useRouter } from "next/router";

const Cadastro = () => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();

  const submit = async (e: SyntheticEvent) => {
    e.preventDefault();

    await fetch('http://localhost:5127/api/Auth/register', {
                    method: "POST",
                    headers: {'Content-Type': 'application/json'},
                    body: JSON.stringify({
                        email,
                        password,
                        name,
      }),
});

    await router.push("/login");
  };

  return (
    <div className={styles.background}>
      <LoginCard title="Crie sua conta">
        <form className={styles.form} onSubmit={submit}>
          <input className={styles.input}
type="text" placeholder="Seu nome"
            required
            onChange={(e) => setName(e.target.value)}
          />

          <input className={styles.input}
type="email" placeholder="Seu e-mail"
            required
            onChange={(e) => setEmail(e.target.value)}
          />

          <input className={styles.input}
type="password" placeholder="Sua senha"
            required
            onChange={(e) => setPassword(e.target.value)}
          />

          <button type="submit" className={styles.button}>Criar
          </button>
          <Link href="/login">Ja possui uma conta?</Link>

        </form>
      </LoginCard>
    </div>
  );
};

export default Cadastro;
