import { UserButton, auth } from "@clerk/nextjs";
import axios from "axios";

export default async function Home() {
  const { getToken } = auth();

  const { data: subscription } = await axios.get(
    "https://localhost:6001/api/Subscription",
    {
      headers: {
        authorization: "Bearer " + (await getToken()),
      },
    }
  );

  return (
    <main className="flex min-h-screen flex-col items-center justify-between p-24">
      <h1 className="text-4xl font-bold">Welcome to your Clerk app!</h1>
      <h2 className="text-2xl font-semibold">Your Subscription</h2>
      <pre className="text-sm">{JSON.stringify(subscription, null, 4)}</pre>
      <UserButton afterSignOutUrl="/" />
      <p className="w-full max-w-3xl break-all">
        Example Token: {await getToken()}
      </p>
      <div className="flex space-x-4">
        <a
          href="https://nextjs.org/docs"
          className="text-xl font-medium text-blue-600"
        >
          Next.js Docs
        </a>
        <a
          href="https://docs.clerk.dev"
          className="text-xl font-medium text-blue-600"
        >
          Clerk Docs
        </a>
      </div>
    </main>
  );
}
