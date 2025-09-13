export default function Contact() {
  const mail = "support@ventize.local";
  return (
    <div className="container card">
      <h2 style={{marginTop:0}}>Contact</h2>
      <p>Need help? Reach us at <a href={`mailto:${mail}`}>{mail}</a>.</p>
    </div>
  );
}
