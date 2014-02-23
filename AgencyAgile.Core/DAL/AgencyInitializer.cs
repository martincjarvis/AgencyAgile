using AgencyAgile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class AgencyInitializer :
        //  System.Data.Entity.DropCreateDatabaseAlways<AgencyDbContext>
        System.Data.Entity.CreateDatabaseIfNotExists<AgencyDbContext>
    {

        private Dictionary<string, ApplicationUser> _users = null;

        private Random _rnd = new Random();

        protected override void Seed(AgencyDbContext context)
        {
            var idCtx = IdentityDbContext.Create(context.SchemaName);
            var system = idCtx.Users.First(u => u.UserName == "System");
            var am = idCtx.Users.First(u => u.UserName == "DemoAccountManager");
            var td = idCtx.Users.First(u => u.UserName == "DemoTechnicalDirector");
            var ta = idCtx.Users.First(u => u.UserName == "DemoTechnicalArchitect");
            var tl = idCtx.Users.First(u => u.UserName == "DemoTechnicalLead");
            var deva = idCtx.Users.First(u => u.UserName == "DemoDeveloperA");
            var devb = idCtx.Users.First(u => u.UserName == "DemoDeveloperB");
            var devc = idCtx.Users.First(u => u.UserName == "DemoDeveloperC");
            var des = idCtx.Users.First(u => u.UserName == "DemoDesigner");
            var ux = idCtx.Users.First(u => u.UserName == "DemoUX");
            var pm = idCtx.Users.First(u => u.UserName == "DemoProjectManager");
            _users = idCtx.Users.Where(u => u.UserName != system.UserName).ToDictionary(u => u.UserName, StringComparer.OrdinalIgnoreCase);


            var briefType = new DocumentType { Name = "Brief", Slug = "brief", SortOrder = 0, LimitPerJob = 1, Description = "An internally interpreted version of initial client provided requirements", Created = AuditedAction.Create(system) };
            briefType.LastUpdated = briefType.Created.Clone();
            context.DocumentTypes.Add(briefType);
            var pitchType = new DocumentType { Name = "Pitch", Slug = "pitch", SortOrder = 1, LimitPerJob = 1, HasFeatures = true, Description = "The initial rough vision of a solution that meets the brief", Created = AuditedAction.Create(system) };
            pitchType.LastUpdated = pitchType.Created.Clone();
            context.DocumentTypes.Add(pitchType);
            var approachType = new DocumentType { Name = "Technical Approach", Slug = "approach", LimitPerJob = 1, HasFeatures = true, HasTasks = true, SortOrder = 2, Description = "The technical counterpart to the pitch, this defines the roughly validated delivery approach that corresponds with any estimates", Created = AuditedAction.Create(system) };
            approachType.LastUpdated = approachType.Created.Clone();
            context.DocumentTypes.Add(approachType);

            var visionType = new DocumentType { Name = "Vision", HasFeatures = true, Slug = "vision", SortOrder = 3, LimitPerJob = 1, Description = "The vision of a solution that meets the brief and sets the boundaries on the scope of work", Created = AuditedAction.Create(system) };
            visionType.LastUpdated = visionType.Created.Clone();
            context.DocumentTypes.Add(visionType);
            context.SaveChanges();
            for (int i = 1; i <= 3; i++)
            {
                var client = new Client
                {
                    Name = string.Format(CultureInfo.InvariantCulture, "Example Client {0}", i)
                    ,
                    Slug = string.Format(CultureInfo.InvariantCulture, "client{0}", i)
                };
                context.Clients.Add(client);
                for (int j = 1; j <= 3; j++)
                {
                    var job = new Job
                    {
                        Client = client,
                        Name = string.Format(CultureInfo.InvariantCulture, "Example Job {0}", i)
                        ,
                        Slug = string.Format(CultureInfo.InvariantCulture, "jn{0:0000}-example-job-{0}", i)
                        ,
                        Reference = string.Format(CultureInfo.InvariantCulture, "JN{0:0000}", i)
                        ,
                        Created = AuditedAction.Create(am)
                    };
                    job.LastUpdated = job.Created.Clone();
                    context.Jobs.Add(job);
                    GenerateDocument(context, client, job, briefType, _rnd.Next(3, 8), _rnd.Next(2, 6));
                    if (_rnd.Next(10) > 4)
                    {
                        GenerateDocument(context, client, job, pitchType, _rnd.Next(3, 8), _rnd.Next(2, 6));
                        if (_rnd.Next(10) > 6)
                        {
                            GenerateDocument(context, client, job, visionType, _rnd.Next(3, 8), _rnd.Next(2, 6));
                            GenerateDocument(context, client, job, approachType, _rnd.Next(3, 8), _rnd.Next(2, 6));
                        }
                    }

                }

            }
            context.SaveChanges();

            base.Seed(context);
        }

        private void GenerateDocument(AgencyDbContext context, Client client, Job job, DocumentType dt, int sectionCount, int qustionCount)
        {
            var document = new Document
            {
                Slug = dt.Slug
                ,
                Title = Headings[_rnd.Next(Headings.Length - 1)].Trim(' ', '.')
                ,
                Client = client
                ,
                Job = job
                ,
                Features = new List<Feature>()
                ,
                Questions = new List<Question>()
                ,
                Sections = new List<CopyBlock>()
                ,
                Created = GenerateRandomAuditedAction()
                ,
                DocumentType = dt
            };
            document.LastUpdated = document.Created.Clone();
            context.Documents.Add(document);
            context.SaveChanges();
            for (int sc = 0; sc < sectionCount; sc++)
            {
                var cb = GenerateStructuredCopyBlockWithHistory(context, true);
                document.Sections.Add(cb);
                document.LastUpdated = cb.History.Last().Created.Clone();
            }
            for (int sc = 0; sc < qustionCount; sc++)
            {
                var cb = GenerateQuestion(context);
                document.Questions.Add(cb);
                document.LastUpdated = cb.LastUpdated.Clone();
            }

            context.SaveChanges();
        }

        private Question GenerateQuestion(AgencyDbContext context)
        {
            var q = new Question
            {
                Text = GenerateCopyBlockWithHistory(context, Headings[_rnd.Next(Headings.Length - 1)].Trim(' ', '.') + "?", Sentences[_rnd.Next(Sentences.Length - 1)].Trim(' ', '.'))
                ,
                Created = GenerateRandomAuditedAction()
                ,
                Responses = new List<Response>()
            };
            q.LastUpdated = q.Created.Clone();
            context.Questions.Add(q);
            context.SaveChanges();
            for (int k = 0; k < _rnd.Next(5) + 1; k++)
            {
                var r = new Response
                {
                    Question = q
                    ,
                    Created = GenerateRandomAuditedAction()
                    ,
                    Text = GenerateCopyBlockWithHistory(context, Headings[_rnd.Next(Headings.Length - 1)], Sentences[_rnd.Next(Sentences.Length - 1)].Trim(' ', '.'))
                };
                r.LastUpdated = r.Created.Clone();
                context.Responses.Add(r);
                context.SaveChanges();
                q.Responses.Add(r);
                q.LastUpdated = r.LastUpdated.Clone();
            }
            if (_rnd.Next(20) % 3 == 0 && q.Responses.Any())
            {
                q.Answer = q.Responses.Last();
                q.Answer.IsAnswer = true;
                context.SaveChanges();
            }
            return q;
        }

        private CopyBlock GenerateStructuredCopyBlockWithHistory(AgencyDbContext context, bool allowSubSections)
        {
            var cb = GenerateCopyBlockWithHistory(context);
            if (allowSubSections && _rnd.Next(10) % 2 == 0)
            {
                for (int k = 0; k < 3; k++)
                {
                    cb.SubSections.Add(GenerateCopyBlockWithHistory(context));
                }
            }
            context.SaveChanges();
            return cb;
        }

        private CopyBlock GenerateCopyBlockWithHistory(AgencyDbContext context)
        {
            var cb = new CopyBlock { History = new List<Fragment>(), SubSections = new List<CopyBlock>() };

            for (int k = 0; k < _rnd.Next(5) + 1; k++)
            {
                var f = new Fragment
                {
                    Created = GenerateRandomAuditedAction()
                    ,
                    Title = Headings[_rnd.Next(Headings.Length - 1)]
                    ,
                    Markup = MarkDown.Transform(Paragraphs[_rnd.Next(Paragraphs.Length - 1)])
                };

                context.Fragments.Add(f);
                context.SaveChanges();
                cb.History.Add(f);
            }
            cb.Latest = cb.History.Last();

            context.CopyBlocks.Add(cb);
            context.SaveChanges();
            return cb;
        }

        private CopyBlock GenerateCopyBlockWithHistory(AgencyDbContext context, string title, string content)
        {
            var cb = new CopyBlock { History = new List<Fragment>(), SubSections = new List<CopyBlock>() };

            var f = new Fragment
            {
                Created = GenerateRandomAuditedAction()
                ,
                Title = title
                ,
                Markup = MarkDown.Transform(content)
            };

            context.Fragments.Add(f);
            context.SaveChanges();
            cb.History.Add(f);

            cb.Latest = cb.History.Last();

            context.CopyBlocks.Add(cb);
            context.SaveChanges();
            return cb;
        }

        private AuditedAction GenerateRandomAuditedAction()
        {
            return AuditedAction.Create(_users.Values.Skip(_rnd.Next(0, _users.Count - 1)).First());
        }



        private static MarkdownDeep.Markdown MarkDown = new MarkdownDeep.Markdown();

        private static string[] Headings = new[]{
            "Lorem ipsum dolor sit amet."
            ,"Nullam consectetur sit amet augue."
            ,"Quisque lacinia euismod turpis ac."
            ,"Nam ornare nisl augue, sit."
            ,"Quisque metus nibh, congue id."
            ,"Quisque lobortis, tellus nec vehicula. "
            ,"Aenean imperdiet sodales enim, molestie. "
            ,"Aenean consectetur libero ut elementum."
        };

        private static string[] Sentences = new[]{
             "Nullam in risus vitae justo laoreet venenatis."
            ,"Integer eget dolor pharetra, mattis eros id, consectetur mi."
            ,"Nam porta est id placerat semper."
            ,"Praesent a lorem eget magna hendrerit cursus eu eu metus."
            ,"Donec sit amet erat ac lacus egestas viverra a tristique lacus."
            ,"Nullam ac metus sit amet est dictum dictum."
            ,"In ut lorem elementum purus dictum accumsan id ut elit."
            ,"Integer ut ligula venenatis, mollis orci vel, tristique quam."
            ,"Fusce id ligula varius sem sollicitudin vehicula et vel est."
            ,"Donec eu metus hendrerit, consectetur sem non, sodales quam."
        };

        private static string[] Paragraphs = new[]{
            "Aliquam vestibulum eros sit amet risus cursus elementum. Nam tristique neque vel euismod vestibulum. Vivamus pretium nec mauris quis faucibus. In hac habitasse platea dictumst. In rutrum pulvinar diam, quis auctor turpis sagittis sed. Cras tincidunt ante nec lorem semper, a tempor tortor luctus. Vestibulum vel arcu facilisis, sollicitudin orci quis, sagittis nisi. Cras ullamcorper dictum ligula sit amet adipiscing. "
            ,"Donec egestas iaculis felis, ac vestibulum turpis consectetur eleifend. In hac habitasse platea dictumst. Aliquam non tellus sapien. Pellentesque at erat at augue ultrices rhoncus. Maecenas id vehicula nunc. Cras consectetur tempor sollicitudin. Aliquam in mauris arcu. Aliquam vitae rhoncus metus. Morbi nisi diam, ultrices id mauris tristique, gravida luctus leo. Curabitur blandit felis non neque lobortis, sed sodales elit consequat. Praesent ac ornare tortor. Phasellus risus tortor, pretium at justo vel, facilisis egestas dui. Vestibulum molestie fringilla bibendum." 
            ,"Donec pellentesque vehicula pretium. Sed in tristique dui. Integer a dolor mi. Suspendisse accumsan mauris sit amet ante bibendum facilisis. Quisque velit enim, tempor sit amet augue vel, ultrices aliquet mauris. Cras ut neque placerat, egestas dui vel, bibendum erat. Morbi imperdiet, sapien nec egestas tincidunt, nibh enim pellentesque mauris, vitae tincidunt augue nisi ut nisl. Quisque at lectus sem. Curabitur tortor odio, hendrerit eu ornare vel, dictum nec lectus. Proin ultrices bibendum nisi, ut mollis odio vestibulum tempus."
            ,"Aliquam eget arcu sed nisl aliquet aliquam in fringilla augue. Praesent blandit ante sit amet dui vulputate, non faucibus libero faucibus. In a pulvinar felis. Proin et sem eu augue blandit volutpat ac et mauris. Cras vitae tellus urna. Aliquam rhoncus ipsum orci, semper rutrum eros aliquet nec. Phasellus quis justo nec neque pulvinar auctor. Vivamus dignissim urna eu rutrum adipiscing. Maecenas euismod lacus quis nibh semper vulputate. Nulla porta dolor ac turpis luctus accumsan. Nam ultrices tincidunt malesuada. Quisque auctor condimentum ante, eget ullamcorper tortor. Nam quis consequat urna, non ultricies mauris. Pellentesque eros nulla, tincidunt varius egestas eu, egestas ut nulla. "
            ,"Mauris nec massa lobortis, luctus orci ut, vulputate quam. Suspendisse at auctor justo, ut imperdiet lectus. Vivamus semper pellentesque turpis, eleifend scelerisque dolor ultricies ac. Mauris eget hendrerit diam. Etiam malesuada quis arcu in semper. Donec pellentesque eu nisl non gravida. Quisque metus metus, malesuada a urna at, suscipit eleifend lacus. Aliquam feugiat id mi eu eleifend. Proin semper vulputate lacus quis lacinia. Nulla porttitor metus sit amet tortor tincidunt, a tempor elit varius. Morbi faucibus purus non nibh tempor, a fringilla nibh pulvinar. Integer faucibus tellus libero, eu adipiscing arcu vestibulum ac."
            ,"Cras vitae risus ac ligula aliquet blandit nec at elit. Vivamus dignissim cursus faucibus. Curabitur sit amet mi non arcu volutpat laoreet ac id orci. Sed lacinia, nunc eget dapibus laoreet, sapien sem congue neque, ut venenatis nulla augue eget nunc. Duis mattis venenatis varius. Mauris tempor dui eu arcu blandit, convallis rhoncus arcu sollicitudin. Suspendisse suscipit ut augue id volutpat. Phasellus mattis fermentum mattis. Sed viverra adipiscing mauris, in sagittis lorem facilisis in. Suspendisse pellentesque justo eu eros placerat dapibus. Phasellus et lectus sit amet ante dapibus gravida sit amet iaculis augue. Cras ornare, metus ultricies adipiscing adipiscing, sem velit aliquet leo, in imperdiet dolor sem ut odio. Proin commodo vitae eros ac blandit. Fusce molestie non sapien nec feugiat. "
        };
    }
}
