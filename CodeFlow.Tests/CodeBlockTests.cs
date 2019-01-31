using Apocalibs.CodeFlow.Fluent;
using NUnit.Framework;
using System;

namespace Tests
{
    public class CodeBlockTests
    {
        private class TestCodeBlock : CodeBlock<TestCodeBlock>
        {
            public TestCodeBlock HelloWorld()
            {
                return SetVar("message", () => "Hello World");
            }

        }


        [Test]
        public void CodeBlock_IfStatement_Scope_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("A", () => false)
                .If((s) => true)
                .Then((s) => s
                    .SetVar("B", () => true)
                    .SetVar("A", () => s.GetVar<bool>("B")))
                .EndIf();

            codeBlock.Execute();

            var ex = Assert.Throws<ArgumentException>(() => codeBlock.GetVar<bool>("B"));
            Assert.AreEqual("Variable 'B' is not defined in current scope", ex.Message);

            Assert.IsTrue(codeBlock.GetVar<bool>("A"));
        }

        [Test]
        public void CodeBlock_IfStatement_Predicate_ThenBranch()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("A", () => 10)
                .SetVar("B", () => 15)
                .SetVar("Error", () => false)
                .If((s) => s.GetVar<int>("A") != s.GetVar<int>("B"))
                .Then((s) => s.SetVar("A", () => s.GetVar<int>("B")))
                .Else((s) => s.SetVar("Error", () => true))
                .EndIf();

            codeBlock.Execute();

            Assert.IsFalse(codeBlock.GetVar<bool>("Error"));
            Assert.AreEqual(codeBlock.GetVar<int>("A"), codeBlock.GetVar<int>("B"));
        }

        [Test]
        public void CodeBlock_IfStatement_Predicate_ElseBranch()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("A", () => 10)
                .SetVar("B", () => 15)
                .SetVar("Error", () => false)
                .If((s) => s.GetVar<int>("A") == s.GetVar<int>("B"))
                .Then((s) => s.SetVar("Error", () => true))
                .Else((s) => s.SetVar("A", () => s.GetVar<int>("B")))
                .EndIf();

            codeBlock.Execute();

            Assert.IsFalse(codeBlock.GetVar<bool>("Error"));
            Assert.AreEqual(codeBlock.GetVar<int>("A"), codeBlock.GetVar<int>("B"));
        }

        [Test]
        public void CodeBlock_WhileStatement_Predicate_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("i", () => 0)
                .While((s) => s.GetVar<int>("i") < 10)
                .Do((s) => s.SetVar("i", () => s.GetVar<int>("i") + 1));

            codeBlock.Execute();

            Assert.AreEqual(10, codeBlock.GetVar<int>("i"));
        }

        [Test]
        public void CodeBlock_WhileStatement_Scope_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("i", () => 0)
                .While((s) => s.GetVar<int>("i") < 10)
                .Do((s) => s
                    .SetVar("i", () => s.GetVar<int>("i") + 1)
                    .SetVar("j", () => s.GetVar<int>("i")));

            codeBlock.Execute();

            var ex = Assert.Throws<ArgumentException>(() => codeBlock.GetVar<bool>("j"));
            Assert.AreEqual("Variable 'j' is not defined in current scope", ex.Message);

            Assert.AreEqual(10, codeBlock.GetVar<int>("i"));
        }

        [Test]
        public void CodeBlock_ForeachStatement_Predicate_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("values", () => new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })
                .SetVar("sum", () => 0)
                .ForEach<int>("value").In("values")
                .Do((s) => s
                    .SetVar("sum", () => s.GetVar<int>("sum") + s.GetVar<int>("value")));

            codeBlock.Execute();

            Assert.AreEqual(55, codeBlock.GetVar<int>("sum"));
        }

        [Test]
        public void CodeBlock_ForeachStatement_Scope_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("values", () => new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })
                .SetVar("sum", () => 0)
                .ForEach<int>("value").In("values")
                .Do((s) => s
                    .SetVar("sum", () => s.GetVar<int>("sum") + s.GetVar<int>("value")));

            codeBlock.Execute();

            var ex = Assert.Throws<ArgumentException>(() => codeBlock.GetVar<int>("value"));
            Assert.AreEqual("Variable 'value' is not defined in current scope", ex.Message);
        }

        [Test]
        public void CodeBlock_HelloWorld_Call_Correct()
        {
            var codeBlock = new TestCodeBlock();

            codeBlock
                .SetVar("message", () => "Goodbye World")
                .HelloWorld();

            codeBlock.Execute();

            Assert.AreEqual("Hello World", codeBlock.GetVar<string>("message"));
        }
    }
}