// AI-generated with assistance
export const DATE_LOCALE = "en-GB"; 
export const MONEY = { locale: "sv-SE", currency: "SEK" };

export const formatDateTime = (iso: string) =>
  new Date(iso).toLocaleString(DATE_LOCALE);

export const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString(DATE_LOCALE);

export const formatMoney = (n: number) =>
  n.toLocaleString(MONEY.locale, { style: "currency", currency: MONEY.currency });
