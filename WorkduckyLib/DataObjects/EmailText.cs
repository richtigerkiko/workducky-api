namespace WorkduckyLib.DataObjects
{
    public class EmailText
    {
        public string Title { get; set; }
        public string Greeting { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }

        /// <summary>
        /// Call To Action Url
        /// </summary>
        /// <value></value>
        public string CTAURL { get; set; }

        /// <summary>
        /// Call to Action Link Text
        /// </summary>
        /// <value></value>
        public string CTAText { get; set; }
        public string Sendoff { get; set; }

        public EmailText()
        {
            Greeting = "Hi there,";
            Sendoff = "GoodBye!";
        }
    }
}